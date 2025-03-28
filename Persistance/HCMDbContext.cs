using Domain;
using Domain.Entities;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance;

public class HCMDbContext : IdentityDbContext<ApplicationUser>
{
    public HCMDbContext(DbContextOptions<HCMDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditable &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var auditable = (IAuditable)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                auditable.CreatedAt = DateTime.UtcNow;
            }

            auditable.LastUpdatedAt = DateTime.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
