namespace Task_DDD.Application.Dto.Employees
{
    public record GetEmployeeLoginDto(Guid Id, string DisplayName, string UserName, bool IsActive);

}
