using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_DDD.Application.Dto.Enums;
using Task_DDD.Common.Helper;
using Task_DDD.Domain.Entity.Employees.ValueObjects;
using Task_DDD.Domain.Entity.Tickets.ValueObjects;
using Task_DDD.Domain.Entity.Users.ValueObjects;

namespace Task_DDD.AdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnumController : ControllerBase
    {

        [HttpGet("ClientUserTypes")]
        public async Task<ActionResult<IList<EnumDto>>> GetClientUserTypeEnum()
        {
            var enumDtos =
                Enum.GetValues(typeof(ClientUserTypeEnum))
                    .Cast<ClientUserTypeEnum>()
                    .Select(v => new EnumDto { Name = v.GetDescription(), Value = (int)v })
                    .ToList();

            return Ok(enumDtos);
        }

        [HttpGet("TicketPriorityTypes")]
        public async Task<ActionResult<IList<EnumDto>>> GetTicketPriorityTypeEnum()
        {
            var enumDtos =
                Enum.GetValues(typeof(TicketPriorityTypeEnum))
                    .Cast<TicketPriorityTypeEnum>()
                    .Select(v => new EnumDto { Name = v.GetDescription(), Value = (int)v })
                    .ToList();

            return Ok(enumDtos);
        }


        [HttpGet("TicketStatusTypes")]
        public async Task<ActionResult<IList<EnumDto>>> GetTicketStatusTypeEnum()
        {
            var enumDtos =
                Enum.GetValues(typeof(TicketStatusTypeEnum))
                    .Cast<TicketStatusTypeEnum>()
                    .Select(v => new EnumDto { Name = v.GetDescription(), Value = (int)v })
                    .ToList();

            return Ok(enumDtos);
        }

        [HttpGet("AdminUserTypes")]
        public async Task<ActionResult<IList<EnumDto>>> GetAdminUserTypeEnum()
        {
            var enumDtos =
                Enum.GetValues(typeof(AdminUserTypeEnum))
                    .Cast<AdminUserTypeEnum>()
                    .Select(v => new EnumDto { Name = v.GetDescription(), Value = (int)v })
                    .ToList();

            return Ok(enumDtos);
        }




    }
}
