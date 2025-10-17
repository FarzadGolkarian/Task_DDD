using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Text.RegularExpressions;
using Task_DDD.Common.Exceptions;

namespace Task_DDD.Common.Helper
{
    public static class PasswordUtility
    {

        public static string GetPassHash(string plainText)
        {
            byte[] salt = System.Text.Encoding.UTF8.GetBytes("p@ssw0rd");

            string hashed
                = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: plainText!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }


        public static void ValidationPassword(string password, string rePassword, int passwordMaxLength, int passwordMinLength)
        {

            if (string.IsNullOrEmpty(password)) throw new BusinessException(ErrorMessages.PasswordIsRequired);

            if (string.IsNullOrEmpty(rePassword)) throw new BusinessException(ErrorMessages.ReNewPasswordRequired);

            if (password.Trim() != rePassword.Trim()) throw new BusinessException(ErrorMessages.NewPasswordAndRePasswordInvalid);

            if (password.Length < passwordMinLength) throw new BusinessException(string.Format(ErrorMessages.PasswordMinLength, passwordMinLength));

            if (password.Length > passwordMaxLength) throw new BusinessException(string.Format(ErrorMessages.PasswordMaxLength, passwordMaxLength));

        }
    }
}
