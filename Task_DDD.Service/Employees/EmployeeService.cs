using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task_DDD.Application.Dto.Employees;
using Task_DDD.Application.RepositoryContracts.Employees;
using Task_DDD.Application.ServiceContracts.Employees;
using Task_DDD.Application.ServiceContracts.Users;
using Task_DDD.Common.Exceptions;
using Task_DDD.Common.Helper;
using Task_DDD.Domain.Entity.Employees.ValueObjects;
using Task_DDD.Service.Base;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Task_DDD.Service.Employees
{
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IConfiguration _configuration;
        public EmployeeService(IUserAuthorizedService userAuthorizedService
            , IEmployeeRepository employeeRepository
            ,IConfiguration configuration) 
            : base(userAuthorizedService)
        {
            _employeeRepository = employeeRepository;
            _configuration = configuration;
        }

        public async Task<GetEmployeeLoginDto> LoginWithPasswordAsync(EmployeeLoginDto dto)
        {

            return await GetEmployeeLoginAsync(dto.UserName, PasswordUtility.GetPassHash(dto.Password));
        }


        public EmployeeLoginAccountDto GenerateToken(HttpRequest request, GetEmployeeLoginDto employeeLoginDto)
        {
            var secretKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                                                            SecurityAlgorithms.HmacSha256Signature);

            var encryptionKey = Encoding.UTF8.GetBytes(_configuration["Jwt:EncryptKey"]);

            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey),
                                                                  SecurityAlgorithms.Aes128KW,
                                                                  SecurityAlgorithms.Aes128CbcHmacSha256);

            Microsoft.Extensions.Primitives.StringValues val;

            request.Headers.TryGetValue(HeaderNames.Authorization, out val);

            var role = ((int)ClientUserTypeEnum.Employee).ToString();

            var claims = new List<Claim>
        {
            new(ClaimTypes.Name,employeeLoginDto.UserName),
            new(ClaimTypes.Actor,role),
            new(ClaimTypes.Role,ClientUserTypeEnum.Employee.ToString()),
            new(ClaimTypes.NameIdentifier,employeeLoginDto.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
        };

            if (!string.IsNullOrEmpty(val))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.CHash, val));
            }
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(180),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(descriptor);

            var encryptedJwt = tokenHandler.WriteToken(securityToken);

            return new EmployeeLoginAccountDto(employeeLoginDto.UserName, employeeLoginDto.DisplayName, encryptedJwt, securityToken.ValidTo);
        }


        private async Task<GetEmployeeLoginDto> GetEmployeeLoginAsync(string userName, string hashPassword)
        {

            var employee = await _employeeRepository.GetQueryable(disableMaxRowLimit:true)
                .FirstOrDefaultAsync
                (s => s.IsActive  && s.Email.ToString() == userName && s.Password == hashPassword);

            if (employee is null) throw new BusinessException(ErrorMessages.UserNotFound);

            return new GetEmployeeLoginDto(employee.Id, employee.FullName, employee.Email, employee.IsActive);

        }



 
    }
}
