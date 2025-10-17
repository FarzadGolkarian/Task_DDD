using Task_DDD.Application.Dto.Users;
using Task_DDD.Application.RepositoryContracts.Users;
using Task_DDD.Application.ServiceContracts.Users;
using Task_DDD.Common.Exceptions;
using Task_DDD.Common.Helper;
using Task_DDD.Domain.Entity.Users;
using Task_DDD.Domain.Entity.Users.ValueObjects;
using Task_DDD.Service.Base;
using Microsoft.EntityFrameworkCore;
using Task_DDD.Application.Dto.BaseDto;


namespace Task_DDD.Service.Users;

public class UserService : BaseService, IUserService
{

    private readonly IUserRepository _userRepository;
    private readonly AdminUserTypeEnum[] _validUserTypes
        = new AdminUserTypeEnum[] { AdminUserTypeEnum.Admin };




    public UserService(IUserAuthorizedService userAuthorizedService,
                        Serilog.ILogger logger,
                       IUserRepository userRepository) : base(userAuthorizedService, logger)
    {
        _userRepository = userRepository;
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