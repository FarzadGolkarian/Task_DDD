using Task_DDD.Domain.Entity.Users.ValueObjects;

namespace Task_DDD.Application.Dto.Users;
public record UserDto(Guid Id,
                      string DisplayName,
                      string UserName,
                      AdminUserTypeEnum UserType,
                      bool IsActive);
