using Task_DDD.Application.ServiceContracts.Users;
using Serilog;


namespace Task_DDD.Service.Base
{
    public class BaseService(IUserAuthorizedService userAuthorizedService)
    {
        protected readonly IUserAuthorizedService UserAuthorizedService = userAuthorizedService;



    }
}
