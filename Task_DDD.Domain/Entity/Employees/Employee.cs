using System.Text.RegularExpressions;
using Task_DDD.Common.Exceptions;
using Task_DDD.Common.Helper;
using Task_DDD.Domain.Common;
using Task_DDD.Domain.Entity.Employees.ValueObjects;
using Task_DDD.Domain.Entity.Tickets;

namespace Task_DDD.Domain.Entity.Employees
{
    public class Employee : BaseEntity<Guid> , IAuditEntity, IModifiedAuditEntity, IActive
    {
        public const int FullNameMaxLength = 100;
        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 20;


        private Employee() { }


        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public ClientUserTypeEnum  ClientUserTypeEnum { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; private set; } = true;

        public virtual List<Ticket> Tickets { get; set; }


        /// <summary>
        /// Default Password is Email , User after login can be change
        /// </summary>

        public static Employee CreateEmployee(string fullName, string email )
        {

            FullNameValidation(fullName);
            EmailValidation(email);

            return new Employee
            {
                FullName = fullName.SafeTrim(),
                Email = PasswordUtility.GetPassHash(email),
                Password = email,
                ClientUserTypeEnum = ClientUserTypeEnum.Employee

            };

        }



        public void UpdateEmployee(string fullName)

        {
            FullNameValidation(fullName); 
        
            FullName = fullName;
        }

        public void SetActive(bool isActive) => IsActive = isActive;
 

        public void ChangeEmployeePassword(string oldPassword, string newPassword, string reNewPassword)
        {


            if (oldPassword.SafeTrim() is null) throw new BusinessException(ErrorMessages.CurrentPasswordIsRequired);

            if (newPassword.SafeTrim() is null) throw new BusinessException(ErrorMessages.NewPasswordRequired);

            if (reNewPassword.SafeTrim() is null) throw new BusinessException(ErrorMessages.ReNewPasswordRequired);
            
            if(!newPassword.SafeTrim().Equals(reNewPassword.SafeTrim())) throw new BusinessException(ErrorMessages.NewPasswordAndRePasswordInvalid);

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
                throw new BusinessException(string.Format(ErrorMessages.EmailIsInvalidFormat , email));

        }



    }

}
