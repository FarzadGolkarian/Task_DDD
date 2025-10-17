using Task_DDD.Application.RepositoryContracts.Tickets;
using Task_DDD.Domain.Entity.Tickets;
using Task_DDD.EF.DatabaseContext;

namespace Task_DDD.Repository.Tickets
{
    public class TicketRepository(TaskDbContext context)
        : GenericRepository<Ticket>(context), ITicketRepository;
}
