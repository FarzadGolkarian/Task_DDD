using System.Text.Json.Serialization;
using Task_DDD.Common.Helper;
using Task_DDD.Domain.Entity.Users.ValueObjects;

namespace Task_DDD.Application.Dto.Users
{
    public record GetDetailUserDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public AdminUserTypeEnum AdminUserType { get; set; }
        public string AdminUserTypeDescription => AdminUserType.GetDescription();
        [JsonIgnore]
        public DateTimeOffset CreateDate { get; init; }
        public string PersianDate => CreateDate.ToShamsiDateString();
        public string CreatedBy { get; set; }

    }

}
