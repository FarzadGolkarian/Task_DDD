using Task_DDD.Domain.Entity.Users.ValueObjects;

namespace Task_DDD.Application.Dto.Users
{
    public record CreateUserDto(string FullName,
                                string Email,
                                string Password,
                                AdminUserTypeEnum AdminUserType);

}
