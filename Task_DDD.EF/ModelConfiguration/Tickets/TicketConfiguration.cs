using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_DDD.Domain.Entity.Employees;
using Task_DDD.Domain.Entity.Tickets;

namespace Task_DDD.EF.ModelConfiguration.Tickets
{
    internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable(nameof(Ticket));


            builder.Property(x => x.Title)
                .HasMaxLength(Ticket.TitleMaxLength)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(Ticket.DescriptionMaxLength)
                ;




        }
    }
}
