using System.ComponentModel;

namespace Task_DDD.Domain.Entity.Tickets.ValueObjects
{
    /// <summary>
    /// Status of Tickets
    /// </summary>
    public enum TicketStatusTypeEnum
    {

        [Description ("Registered")]
        Open = 2,

        [Description ("Pending")]
        InProgress = 4,

        [Description ("Closed")]
        Closed = 8,
    }
}
