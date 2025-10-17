using System.Text.Json.Serialization;
using Task_DDD.Common.Helper;
using Task_DDD.Domain.Entity.Employees;
using Task_DDD.Domain.Entity.Tickets.ValueObjects;
using Task_DDD.Domain.Entity.Users;

namespace Task_DDD.Application.Dto.Tickets
{
    public record GetAllTicketDto
    {
        public string Title { get; init; }

        public string Description { get; init; }

        [JsonIgnore]
        public TicketStatusTypeEnum TicketStatusType { get; init; }
        public string TicketStatusTypeDescription => TicketStatusType.GetDescription();

        [JsonIgnore]
        public TicketPriorityTypeEnum TicketPriorityType { get; init; }
        public string TicketPriorityTypeDescription => TicketPriorityType.GetDescription();

        [JsonIgnore]
        public DateTimeOffset CreatedAt { get; init; }
        public string CreatedAtPersianDate => CreatedAt.ToShamsiDateString();

        [JsonIgnore]
        public DateTimeOffset? UpdatedAt { get; init; }
        public string UpdatedAtPersianDate => UpdatedAt?.ToShamsiDateString();


        public string? UpdatedBy { get; init; }
        public bool IsActive { get; init; } 


        public string AssignedToUser { get; init; }
        public string CreatedByUser { get; init; }


    }
}
