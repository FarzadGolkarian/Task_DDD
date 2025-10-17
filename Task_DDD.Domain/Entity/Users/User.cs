using System.Text.RegularExpressions;
using Task_DDD.Common.Exceptions;
using Task_DDD.Common.Helper;
using Task_DDD.Domain.Common;
using Task_DDD.Domain.Entity.Tickets;
using Task_DDD.Domain.Entity.Users.ValueObjects;

namespace Task_DDD.Domain.Entity.Users
{
    public class User : BaseEntity<Guid>, IAuditEntity, IModifiedAuditEntity, IActive 
    {
        public const int FullNameMaxLength = 100;
        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 20;

        private User() { }

        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public AdminUserTypeEnum  AdminUserType { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;
        public virtual List<Ticket>  Tickets { get; set; }



        public static User CreateUser(string fullName,
                                      string email,
                                      string password,
                                      AdminUserTypeEnum adminUserType)
        {

            EnumUtility.ValidationEnumDefined(typeof(AdminUserTypeEnum), adminUserType, "Admin Type");
            FullNameValidation(fullName);
            EmailValidation(email);

            return new User()
            {
                FullName = fullName,
                Email = email,
                Password = password,
                AdminUserType = adminUserType
            };
        }



        public void SetIsDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void Update(string fullName , AdminUserTypeEnum adminUserType)

        {
            FullNameValidation(fullName);
            FullName = fullName;
            EnumUtility.ValidationEnumDefined(typeof(AdminUserTypeEnum), adminUserType, "Admin Type");
            AdminUserType = adminUserType;

        }

        public void ChangePassword(string password)
        {

            Password = PasswordUtility.GetPassHash(password.SafeTrim());
        }

        public void ChangeAdminPassword(string oldPassword, string newPassword, string reNewPassword)
        {


            if (oldPassword.SafeTrim() is null) throw new BusinessException(ErrorMessages.CurrentPasswordIsRequired);

            if (newPassword.SafeTrim() is null) throw new BusinessException(ErrorMessages.NewPasswordRequired);

            if (reNewPassword.SafeTrim() is null) throw new BusinessException(ErrorMessages.ReNewPasswordRequired);

            if (!newPassword.SafeTrim().Equals(reNewPassword.SafeTrim())) throw new BusinessException(ErrorMessages.NewPasswordAndRePasswordInvalid);

            var hashPassword = PasswordUtility.GetPassHash(oldPassword.SafeTrim());

            if (hashPassword != Password) throw new BusinessException(ErrorMessages.CurrentPasswordInvalid);

            PasswordUtility.ValidationPassword(newPassword, reNewPassword, PasswordMaxLength, PasswordMinLength);

            Password = PasswordUtility.GetPassHash(newPassword.SafeTrim());
        }

        private static void FullNameValidation(string fullName)
        {

            if (string.IsNullOrWhiteSpace(fullName))
                throw new BusinessException(ErrorMessages.FullNameIsInvalid);

            if (fullName.Length > FullNameMaxLength)
                throw new BusinessException(string.Format(ErrorMessages.FullNameMaxLength, FullNameMaxLength));

        }

        private static void EmailValidation(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
                throw new BusinessException((ErrorMessages.EmailIsInvalid));

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new BusinessException(string.Format(ErrorMessages.EmailIsInvalidFormat, email));

        }

    }
}
