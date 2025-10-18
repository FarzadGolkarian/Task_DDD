using Microsoft.AspNetCore.Http;
using Task_DDD.Application.Dto.Users;
using Task_DDD.Domain.Entity.Users.ValueObjects;

namespace Task_DDD.Application.ServiceContracts.Users;

public interface IUserService
{

    Task<UserDto> LoginWithPasswordAsync(LoginDto dto);
    LoginAccountDto GenerateToken(HttpRequest request, UserDto userDto);
    Task<UserDto> GetUserAsync(AdminUserTypeEnum userType, string userName, string password);

}