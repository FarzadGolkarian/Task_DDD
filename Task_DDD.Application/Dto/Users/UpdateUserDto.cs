using Task_DDD.Domain.Entity.Users.ValueObjects;

namespace Task_DDD.Application.Dto.Users
{
    public record UpdateUserDto(string FullName, AdminUserTypeEnum AdminUserTypeEnum);

}
