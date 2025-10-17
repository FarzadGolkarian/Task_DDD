using Task_DDD.Application.RepositoryContracts.Employees;
using Task_DDD.Domain.Entity.Employees;
using Task_DDD.EF.DatabaseContext;

namespace Task_DDD.Repository.Employees
{
    public class EmployeeRepository(TaskDbContext context) 
        :GenericRepository<Employee>(context) , IEmployeeRepository;

}
