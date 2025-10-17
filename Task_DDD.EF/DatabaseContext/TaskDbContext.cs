using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Task_DDD.Common.Exceptions;
using Task_DDD.Domain.Common;
using Task_DDD.Domain.Entity.Employees;
using Task_DDD.Domain.Entity.Tickets;
using Task_DDD.Domain.Entity.Users;

namespace Task_DDD.EF.DatabaseContext
{
    public class TaskDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaskDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor)
    : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            ConfigAuditEntity(modelBuilder);
            ConfigModifiedAuditEntity(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SetAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }



        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Ticket> Tickets { get; set; }









        private static void ConfigAuditEntity(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model
                .GetEntityTypes()
                .Where(e => typeof(IAuditEntity)
                .IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entity.Name)
                    .Property(nameof(IAuditEntity.CreatedBy))
                    .HasMaxLength(100);

                modelBuilder.Entity(entity.Name)
                    .Property(nameof(IAuditEntity.CreatedAt))
                    .IsRequired();
            }
        }

        private static void ConfigModifiedAuditEntity(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes().Where(e => typeof(IModifiedAuditEntity).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entity.Name)
                    .Property(nameof(IModifiedAuditEntity.UpdatedBy))
                    .IsRequired(false).HasMaxLength(100);

                modelBuilder.Entity(entity.Name)
                    .Property(nameof(IModifiedAuditEntity.UpdatedAt))
                    .IsRequired(false);
            }
        }


        void SetAudit()
        {
            var addedAuditedEntities =
                ChangeTracker.Entries<IAuditEntity>()
                             .Where(p => p.State == EntityState.Added)
                             .Select(p => p.Entity);

            var now = DateTime.UtcNow;

            string? user = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedAt = now;

                if (added is IUser u)
                {
                    if (u != null)
                    {
                        added.CreatedBy = u.UserName;

                        continue;
                    }
                }
                added.CreatedBy = user ?? 
                    throw new BusinessException(ErrorMessages.UserNotFound);
            }
        }

    }
}
