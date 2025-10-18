using Task_DDD.Application.Dto.Tickets;
using Task_DDD.Domain.Entity.Tickets.ValueObjects;

namespace Task_DDD.Application.ServiceContracts.Tickets
{
    public interface ITicketService 
    {
        Task<Guid> CreateTicketAsync(CreateTicketDto dto);
        Task<List<GetAllTicketDto>> CurrentUserTicketListAsync();
        Task<List<GetAllTicketDto>> GetAllTicketListAsync();
        Task UpdateTicketAsync(Guid ticketId , UpdateTicketDto dto);
        Task ChangeTicketStatusAsync(Guid ticketId, TicketStatusTypeEnum status);
        Task DeleteTicketAsync(Guid ticketId);
        Task<GetAllTicketDto> GetDetailTicketAsync(Guid ticketId);
    }
}
