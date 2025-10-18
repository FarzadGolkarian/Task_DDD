using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_DDD.Domain.Entity.Users;

namespace Task_DDD.EF.ModelConfiguration.Users
{
    internal class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.Property(x => x.FullName)
                .HasMaxLength(User.FullNameMaxLength)
                .IsRequired();

            builder.Property(x => x.Email)
                .IsRequired();

            builder.HasMany(x => x.Tickets)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.AssignedToUserId)
                .OnDelete(DeleteBehavior.Cascade)
                ;

        }
    }
}
