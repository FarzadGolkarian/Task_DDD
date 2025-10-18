using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_DDD.Application.Dto.Employees;
using Task_DDD.Application.ServiceContracts.Employees;

namespace Task_DDD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeAccountController : BaseController
    {

        private readonly IEmployeeService _employeeService;

        public EmployeeAccountController(IEmployeeService employeeService)
        {

            _employeeService = employeeService;
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<EmployeeLoginAccountDto>> Login([FromBody] EmployeeLoginDto dto)
        {
            var employee = await _employeeService.LoginWithPasswordAsync(dto);

            var result = _employeeService.GenerateToken(Request, employee);

            return Ok(result);
        }


    }
}
