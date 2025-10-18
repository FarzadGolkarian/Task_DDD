using Task_DDD.Domain.Entity.Users.ValueObjects;

namespace Task_DDD.Application.Dto.Users
{
    public record LoginAccountDto(string DisplayName,
                                  AdminUserTypeEnum UserType,
                                  string Token,
                                  DateTimeOffset ExpireTime);
}
