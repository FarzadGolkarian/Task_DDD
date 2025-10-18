using Microsoft.AspNetCore.Http;
using Task_DDD.Application.Dto.Employees;

namespace Task_DDD.Application.ServiceContracts.Employees
{
    public interface IEmployeeService
    {
        Task<GetEmployeeLoginDto> LoginWithPasswordAsync(EmployeeLoginDto dto);


        EmployeeLoginAccountDto GenerateToken(HttpRequest request, GetEmployeeLoginDto employeeLoginDto);

    }
}
