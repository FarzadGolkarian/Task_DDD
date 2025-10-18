using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task_DDD.Application.Dto.BaseDto;
using Task_DDD.Application.Dto.Users;
using Task_DDD.Application.RepositoryContracts.Users;
using Task_DDD.Application.ServiceContracts.Users;
using Task_DDD.Common.Exceptions;
using Task_DDD.Common.Helper;
using Task_DDD.Domain.Entity.Users;
using Task_DDD.Domain.Entity.Users.ValueObjects;
using Task_DDD.Service.Base;


namespace Task_DDD.Service.Users;

public class UserService : BaseService, IUserService
{

    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly AdminUserTypeEnum[] _validUserTypes
        = new AdminUserTypeEnum[] { AdminUserTypeEnum.Admin };




    public UserService(IUserAuthorizedService userAuthorizedService,
                       IConfiguration configuration,
                       IUserRepository userRepository) : base(userAuthorizedService)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<UserDto> LoginWithPasswordAsync(LoginDto dto)
    {
        return  await GetUserAsync(dto.UserType, dto.Email, dto.Password);
    }

    public  LoginAccountDto GenerateToken(HttpRequest request, UserDto userDto)
    {

        var secretKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),

            SecurityAlgorithms.HmacSha256Signature);

        var encryptionkey = Encoding.UTF8.GetBytes(_configuration["Jwt:EncryptKey"]);

        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey),

            SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        Microsoft.Extensions.Primitives.StringValues val;

        request.Headers.TryGetValue(HeaderNames.Authorization, out val);

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.UserName),
                new Claim(ClaimTypes.Actor, userDto.UserType.ToString()),
                new Claim(ClaimTypes.Role, userDto.UserType.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserType", userDto.UserType.ToString()),
            };

        if (!string.IsNullOrEmpty(val))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.CHash, val));
        }


        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(180),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(descriptor);

        string encryptedJwt = tokenHandler.WriteToken(securityToken);

        return new LoginAccountDto(userDto.DisplayName, userDto.UserType, encryptedJwt, securityToken.ValidTo);
    }

    public async Task<UserDto> GetUserAsync(AdminUserTypeEnum userType, string userName, string password)
    {
        var hashPassword = PasswordUtility.GetPassHash(password);

        var user
            = await _userRepository.GetQueryable(disableMaxRowLimit: true)
                .Where(x => x.Email == userName && x.Password == hashPassword)
                .FirstOrDefaultAsync();

        if (user == null) throw new BusinessException(string.Format(ErrorMessages.UserNotFoundByID, userName));


        return new UserDto(user.Id, user.FullName, user.Email, user.AdminUserType, user.IsActive);
    }




    public async Task ChangeCurrentUserPasswordAsync(ChangeUserPasswordDto dto)
    {
        var userId = UserAuthorizedService.UserId;

        var user = await _userRepository.GetAsync(userId);

        if (user == null) throw new BusinessException(ErrorMessages.UserNotFound);

        var hashPassword = PasswordUtility.GetPassHash(dto.CurrentPassword);

        if (hashPassword != user.Password) throw new BusinessException(ErrorMessages.CurrentPasswordInvalid);

        PasswordUtility.ValidationPassword(dto.NewPassword, dto.RePassword, User.PasswordMaxLength, User.PasswordMinLength);

        user.ChangePassword(dto.NewPassword);

        await _userRepository.Update(user);
    }

    public async Task ChangePasswordAsync(Guid id, ChangePasswordDto dto)
    {
        var user = await _userRepository.GetQueryable(disableGlobalFilter: true)
            .FirstOrDefaultAsync(a => a.Id == id && _validUserTypes
            .Contains(a.AdminUserType));

        if (user is null) throw new BusinessException(string.Format(ErrorMessages.UserNotFoundByID, id));

        user.ChangePassword(dto.Password);

        await _userRepository.Update(user);

    }

    public async Task ChangeStatusAsync(Guid id, ChangeStatusDto dto)
    {
        var user = await _userRepository.GetQueryable(disableGlobalFilter: true)
            .FirstOrDefaultAsync(a => a.Id == id && _validUserTypes
            .Contains(a.AdminUserType));

        if (user is null) throw new BusinessException(string.Format(ErrorMessages.UserNotFoundByID, id));

        user.SetIsActive(dto.IsActive);

        await _userRepository.Update(user);

    }

    public async Task<Guid> CreateAsync(CreateUserDto dto)
    {
        User user;

        user = User.CreateUser(
           fullName: dto.FullName,
           email: dto.Email,
           password: dto.Password,
           adminUserType: dto.AdminUserType);

        await _userRepository.Add(user);

        return user.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetQueryable(disableGlobalFilter: true)
                    .FirstOrDefaultAsync(a => a.Id == id && _validUserTypes
                    .Contains(a.AdminUserType));

        if (user is null) throw new BusinessException(string.Format(ErrorMessages.UserNotFoundByID, id));

        user.SetIsDeleted(true);
        await _userRepository.Update(user);
    }



    public async Task<GetDetailUserDto> GetByIdAsync(Guid id)
    {
        var query = _userRepository.GetQueryable(disableMaxRowLimit: true);

        var user = await query
             .Where(a => a.Id == id && _validUserTypes.Contains(a.AdminUserType))
            .Select(a => new GetDetailUserDto
            {
                FullName = a.FullName,
                AdminUserType = a.AdminUserType,
                CreateDate = a.CreatedAt,
                CreatedBy = a.CreatedBy,
                IsActive = a.IsActive,
                UserName = a.Email


            }).FirstOrDefaultAsync();

        if (user is null) throw new BusinessException(string.Format(ErrorMessages.UserNotFoundByID, id));

        return user;
    }

    public async Task<GetUserInfoDto> GetCurrentUserInfoAsync()
    {
        var userType = base.UserAuthorizedService.UserType;

        var userId = base.UserAuthorizedService.UserId;

        var user = await _userRepository.GetQueryable(disableMaxRowLimit: true)
            .FirstOrDefaultAsync(x => x.Id == userId && x.AdminUserType == userType);

        if (user == null) throw new BusinessException(string.Format(ErrorMessages.UserNotFoundByID, userId));

        return new GetUserInfoDto(user.Email, user.FullName);
    }



    public async Task UpdateAsync(Guid id, UpdateUserDto dto)
    {

        var user = await _userRepository.GetQueryable(disableMaxRowLimit: true)
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (user is null) throw new BusinessException(string.Format (ErrorMessages.UserNotFoundByID,id));

        user.Update(
                    dto.FullName,
                    dto.AdminUserTypeEnum
                    );

        await _userRepository.Update(user);
    }
}