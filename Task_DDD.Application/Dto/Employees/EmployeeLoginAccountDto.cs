namespace Task_DDD.Application.Dto.Employees
{
    public record EmployeeLoginAccountDto(string UesrName,
                                            string displayName,
                                            string Token,
                                            DateTimeOffset ExpireTime);
}
