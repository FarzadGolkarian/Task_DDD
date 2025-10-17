namespace Task_DDD.Domain.Common
{
    public interface IAuditEntity
    {
        ///<summary>
        ///تاریخ آیجاد
        /// </summary>
         DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// ایجاد کننده
        /// </summary>
        string CreatedBy { get; set; }
    }
}
