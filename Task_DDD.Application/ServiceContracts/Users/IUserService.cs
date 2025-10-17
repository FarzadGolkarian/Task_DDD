using Task_DDD.Application.Dto.BaseDto;
using Task_DDD.Application.Dto.Users;
using Task_DDD.Domain.Entity.Users.ValueObjects;

namespace Task_DDD.Application.ServiceContracts.Users;

public interface IUserService
{
    Task<GetUserInfoDto> GetCurrentUserInfoAsync();
    Task<UserDto> GetUserAsync(AdminUserTypeEnum userType, string userName, string password);
    Task ChangeCurrentUserPasswordAsync(ChangeUserPasswordDto dto);
    Task<GetDetailUserDto> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateUserDto dto);
    Task UpdateAsync(Guid id, UpdateUserDto dto);
    Task DeleteAsync(Guid id);
    Task ChangeStatusAsync(Guid id, ChangeStatusDto dto);
    Task ChangePasswordAsync(Guid id, ChangePasswordDto dto);
}