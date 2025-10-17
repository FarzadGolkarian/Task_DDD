using Task_DDD.Application.RepositoryContracts.Users;
using Task_DDD.Domain.Entity.Users;
using Task_DDD.EF.DatabaseContext;

namespace Task_DDD.Repository.Users

{
    public class UserRepository(TaskDbContext context)
        : GenericRepository<User>(context), IUserRepository;
}
