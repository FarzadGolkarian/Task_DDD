using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_DDD.Application.Dto.BaseDto;
using Task_DDD.Application.Dto.Tickets;
using Task_DDD.Application.ServiceContracts.Tickets;

namespace Task_DDD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }




        [HttpPost()]
        public async Task<ActionResult<BaseReturnDto>> CreateTicket([FromBody] CreateTicketDto dto)
        {
            return Ok(new BaseReturnDto(await _ticketService.CreateTicketAsync(dto)));
        }




        [HttpGet()]
        public async Task<ActionResult<List<GetAllTicketDto>>> CurrentUserTicketList()
        {
            return Ok(await _ticketService.CurrentUserTicketListAsync());
        }

    }
}
