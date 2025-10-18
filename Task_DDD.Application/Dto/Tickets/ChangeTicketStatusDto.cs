using Task_DDD.Domain.Entity.Tickets.ValueObjects;

namespace Task_DDD.Application.Dto.Tickets
{
    public record ChangeTicketStatusDto(TicketStatusTypeEnum status);

}
