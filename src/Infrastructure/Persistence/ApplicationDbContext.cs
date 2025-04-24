using System.Reflection;
using System.Text.Json;
using DealClean.Application.Common.Interfaces;
using DealClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{

    public DbSet<Deal> Deals { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Deal>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Video)
              .HasColumnType("jsonb")
              .IsRequired(false);
    });

        builder.Entity<Hotel>(entity =>
            {
                entity.HasKey(e => e.HotelId);

                // Configure Media as JSONB column
                entity.Property(e => e.Media)
                      .HasColumnType("jsonb")
                      .IsRequired(false);
            });

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }


}