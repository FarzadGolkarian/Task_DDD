using Task_DDD.Domain.Entity.Users.ValueObjects;

namespace Task_DDD.Application.Dto.Users;

public record LoginDto(AdminUserTypeEnum UserType, string Email, string Password);