using System.Security.Claims;
using Task_DDD.Domain.Entity.Users.ValueObjects;
namespace Task_DDD.Application.ServiceContracts.Users
{
    public interface IUserAuthorizedService
    {
        ClaimsPrincipal User { get; }
        bool IsAuthenticated { get; }
        Guid UserId { get; }
        AdminUserTypeEnum? UserType { get; }
        Guid GetEmployeeId();
        void VerifyUserType(params AdminUserTypeEnum[] validUserTypes);
    }
}
