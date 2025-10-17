using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_DDD.Application.Dto.Employees
{
    public record EmployeeLoginDto
    {
        private string _userName;
        private string _password;
        public string UserName
        {
            get => _userName.Trim();
            set => _userName = value;

        }

        public string Password
        {
            get => _password.Trim();
            set => _password = value;
        }
    }
}
