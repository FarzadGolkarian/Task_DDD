using Task_DDD.Domain.Entity.Tickets.ValueObjects;

namespace Task_DDD.Application.Dto.Tickets
{
    public record CreateTicketDto(string Title,
                                          string Description,
                                          Guid AssignedToUserId,
                                          TicketPriorityTypeEnum TicketPriorityType);


}
