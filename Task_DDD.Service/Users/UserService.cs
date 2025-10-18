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


}