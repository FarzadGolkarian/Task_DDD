namespace Task_DDD.Domain.Common
{

    public interface IModifiedAuditEntity
    {
        /// <summary>
        /// تاریخ آخرین تغییر
        /// </summary>
        DateTimeOffset? UpdatedAt { get; set; }
        /// <summary>
        /// تغییر دهنده
        /// </summary>
        string? UpdatedBy { get; set; }
    }
}