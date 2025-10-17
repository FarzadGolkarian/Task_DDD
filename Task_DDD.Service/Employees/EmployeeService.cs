using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Text;
using Task_DDD.Application.Dto.Employees;
using Task_DDD.Application.RepositoryContracts.Employees;
using Task_DDD.Application.ServiceContracts.Employees;
using Task_DDD.Application.ServiceContracts.Users;
using Task_DDD.Common.Exceptions;
using Task_DDD.Common.Helper;
using Task_DDD.Service.Base;

namespace Task_DDD.Service.Employees
{
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IUserAuthorizedService userAuthorizedService
            , IEmployeeRepository employeeRepository
            , ILogger logger) 
            : base(userAuthorizedService, logger)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<GetEmployeeLoginDto> LoginWithPasswordAsync(EmployeeLoginDto dto)
        {

            return await GetEmployeeLoginAsync(dto.UserName, PasswordUtility.GetPassHash(dto.Password));
        }

        private async Task<GetEmployeeLoginDto> GetEmployeeLoginAsync(string userName, string hashPassword)
        {

            var employee = await _employeeRepository.GetQueryable(disableMaxRowLimit:true)
                .FirstOrDefaultAsync
                (s => s.IsActive == true && s.Email.ToString() == userName && s.Password == hashPassword);

            if (employee is null) throw new BusinessException(ErrorMessages.UserNotFound);

            return new GetEmployeeLoginDto(employee.Id, employee.FullName, employee.Email, employee.IsActive);

        }

    }
}
