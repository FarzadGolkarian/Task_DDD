using System.ComponentModel;

namespace Task_DDD.Domain.Entity.Tickets.ValueObjects
{
    /// <summary>
    /// Status of Tickets Priority
    /// </summary>
    public enum TicketPriorityTypeEnum
    {

        [Description ("Low")]
        Low = 2,

        [Description ("Medium")]
        Medium = 4,

        [Description ("High")]
        High = 8,
    }
}
