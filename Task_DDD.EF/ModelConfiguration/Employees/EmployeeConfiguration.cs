using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_DDD.Domain.Entity.Employees;

namespace Task_DDD.EF.ModelConfiguration.Employees
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(nameof(Employee));


            builder.Property(x => x.FullName)
                .HasMaxLength(Employee.FullNameMaxLength)
                .IsRequired();

            builder.Property(x => x.Email)
                .IsRequired();

            builder.Property(x => x.Password)
                .IsRequired();

            builder.HasMany(x => x.Tickets)
                .WithOne(x => x.Employee)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
