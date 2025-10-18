using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_DDD.Application.Dto.Users;
using Task_DDD.Application.ServiceContracts.Users;

namespace Task_DDD.AdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;

        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginAccountDto>> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.LoginWithPasswordAsync(dto);

            var result = _userService.GenerateToken(Request, user);

            return Ok(result);


        }






    }
}
