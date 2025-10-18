using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Task_DDD.Application.ServiceContracts.Users;
using Task_DDD.Common.Exceptions;
using Task_DDD.Domain.Entity.Employees.ValueObjects;
using Task_DDD.Domain.Entity.Users.ValueObjects;

namespace Task_DDD.Service.Users
{
    public class AuthorizeUserService(IHttpContextAccessor accessor) : IUserAuthorizedService
    {
        public ClaimsPrincipal User => accessor.HttpContext.User as ClaimsPrincipal;
       
        public bool IsAuthenticated => User != null ? User.Identity.IsAuthenticated : false;

        public Guid UserId
        {
            get
            {
                var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier);
                if (nameIdentifier == null)
                    throw new BusinessException(string.Format(ErrorMessages.TryAgain));

                if (!Guid.TryParse(nameIdentifier.Value, out Guid userId))
                    throw new BusinessException(string.Format(ErrorMessages.TokenIsNotValid));

                return userId;
            }
        }
        public AdminUserTypeEnum? UserType
        {
            get
            {
                var actor = User.FindFirst(ClaimTypes.Actor);
                if (actor == null)
                    return null;

                return (AdminUserTypeEnum)Enum.Parse(typeof(AdminUserTypeEnum), actor.Value);
            }
        }

        public void VerifyUserType(params AdminUserTypeEnum[] validUserTypes)
        {
            if (UserType == null)
                throw new BusinessException(string.Format(ErrorMessages.AccessIsNotpermitted));

            if (validUserTypes?.Contains(UserType.Value) != true)
            {
                throw new BusinessException(string.Format(ErrorMessages.AccessIsNotpermitted));
            }
        }

        public Guid GetEmployeeId()
        {
            var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifier == null)
                throw new BusinessException(string.Format(ErrorMessages.TryAgain));

            if (!Guid.TryParse(nameIdentifier.Value, out Guid userId))
                throw new BusinessException(string.Format(ErrorMessages.TokenIsNotValid));


            var actor = User.FindFirst(ClaimTypes.Actor);
            if (actor == null)
                throw new BusinessException(string.Format(ErrorMessages.TryAgain));

            var userType = (ClientUserTypeEnum)Enum.Parse(typeof(ClientUserTypeEnum), actor.Value);

            if (userType == ClientUserTypeEnum.Employee)
                return userId;
            throw new BusinessException(string.Format(ErrorMessages.AccessIsNotpermitted));
        }


    }
}
