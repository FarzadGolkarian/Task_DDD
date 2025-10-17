using System.ComponentModel;

namespace Task_DDD.Domain.Entity.Employees.ValueObjects
{
    /// <summary>
    /// نوع کاربر در پنل 
    /// </summary>
    public enum ClientUserTypeEnum
    {

        [Description ("Client")]
        Employee = 20,
    }
}
