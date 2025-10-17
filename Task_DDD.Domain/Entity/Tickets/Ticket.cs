using Task_DDD.Common.Exceptions;
using Task_DDD.Common.Helper;
using Task_DDD.Domain.Common;
using Task_DDD.Domain.Entity.Employees;
using Task_DDD.Domain.Entity.Tickets.ValueObjects;
using Task_DDD.Domain.Entity.Users;

namespace Task_DDD.Domain.Entity.Tickets
{
    public class Ticket : BaseEntity<Guid>, IAuditEntity, IModifiedAuditEntity, IActive
    {

        public const int TitleMaxLength = 50;
        public const int DescriptionMaxLength = 250;



        public string Title { get; private set; }
        public string Description { get; private set; }
        public TicketStatusTypeEnum TicketStatusTypeEnum { get; private set; }
        public TicketPriorityTypeEnum TicketPriorityTypeEnum { get; private set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; private set; } = true;

        /// <summary>
        /// Relation To User
        /// </summary>
        public virtual User User { get; set; }
        public Guid AssignedToUserId { get; set; }

        /// <summary>
        /// Relation To Employee
        /// </summary>
        public virtual Employee Employee { get; set; }
        public Guid CreatedByUserId { get; set; }

        private Ticket() { }


        public static Ticket CreateTicket(string title,
                                          string description,
                                          Guid assignedToUserId,
                                          TicketPriorityTypeEnum ticketPriorityTypeEnum)
        {
            TitleValidation(title);
            DescriptionValidation(description);
            GuidValidation(assignedToUserId);
            EnumUtility.ValidationEnumDefined(typeof(TicketPriorityTypeEnum), ticketPriorityTypeEnum, "Status of Tickets Priority");

            return new Ticket()
            {
                Title = title.SafeTrim(),
                Description = description.SafeTrim(),
                AssignedToUserId = assignedToUserId,
                TicketPriorityTypeEnum = ticketPriorityTypeEnum,
                TicketStatusTypeEnum = TicketStatusTypeEnum.Open
            };
        }


        public void UpdateTicket(string title,
                                          string description,
                                          Guid assignedToUserId,
                                          TicketPriorityTypeEnum ticketPriorityTypeEnum)
        {
            TitleValidation(title);
            Title = title.SafeTrim();
            DescriptionValidation(description);
            Description = description.SafeTrim();
            GuidValidation(assignedToUserId);
            AssignedToUserId = assignedToUserId;
            EnumUtility.ValidationEnumDefined(typeof(TicketPriorityTypeEnum), ticketPriorityTypeEnum, "Status of Tickets Priority");
            TicketPriorityTypeEnum = ticketPriorityTypeEnum;

        }


        public void UpdateTicketStatus(TicketStatusTypeEnum ticketStatusType)
        {
            EnumUtility.ValidationEnumDefined(typeof(TicketStatusTypeEnum), ticketStatusType, "Status of Tickets");

            TicketStatusTypeEnum = ticketStatusType;
        }



        private static void TitleValidation(string title)
        {
            title = title.SafeTrim();

            if (string.IsNullOrWhiteSpace(title)) throw new BusinessException(string.Format(ErrorMessages.TitleIsRequired));

            if (title.Length > TitleMaxLength) throw new BusinessException(string.Format(ErrorMessages.TitleMaxLength, TitleMaxLength));

        }
        private static void DescriptionValidation(string description)
        {
            description = description.SafeTrim();

            if (description.Length > DescriptionMaxLength)
                throw new BusinessException(string.Format(ErrorMessages.DescriptionMaxLength, DescriptionMaxLength));

        }

        private static void GuidValidation(Guid guid)
        {
            if (guid == Guid.Empty)
            {
                throw new BusinessException(ErrorMessages.GuidIsInvalidFormat);
            }
        }

    }
}
