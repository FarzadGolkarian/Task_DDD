using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_DDD.Application.Dto.BaseDto;
using Task_DDD.Application.Dto.Tickets;
using Task_DDD.Application.ServiceContracts.Tickets;
using Task_DDD.Domain.Entity.Tickets.ValueObjects;
using Task_DDD.Service.Tickets;

namespace Task_DDD.AdminWebApi.Controllers
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



        [HttpGet()]
        public async Task<ActionResult<List<GetAllTicketDto>>> GetAllTicketList()
        {
            return Ok(await _ticketService.GetAllTicketListAsync());
        }



        [HttpGet("{ticketId:guid}")]
        public async Task<ActionResult<GetAllTicketDto>> GetDetailTicket(Guid ticketId)
        {
            return Ok(await _ticketService.GetDetailTicketAsync(ticketId));
        }
 

        [HttpPut("{ticketId:guid}/ChangeStatus")]
        public async Task<ActionResult> ChangeTicketStatus(Guid ticketId, [FromBody] ChangeTicketStatusDto dto)
        {
            await _ticketService.ChangeTicketStatusAsync(ticketId, dto);
            return Ok();
            
        }


        [HttpPut("{ticketId:guid}")]
        public async Task<ActionResult> UpdateTicket(Guid ticketId, [FromBody] UpdateTicketDto dto)
        {
            await _ticketService.UpdateTicketAsync(ticketId, dto);
            return Ok();
        }


        [HttpDelete("{ticketId:guid}")]
        public async Task<ActionResult> DeleteTicket(Guid ticketId)
        {
            await _ticketService.DeleteTicketAsync(ticketId);
            return Ok();
        }






    }
}


