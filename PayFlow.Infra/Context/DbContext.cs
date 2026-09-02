using Microsoft.EntityFrameworkCore;
using PayFlow.domain.Models.Payment;
using PayFlow.Domain.Entities;

namespace PayFlow.Infra.Context;

public class PayFlowDbContext : DbContext
{
    public PayFlowDbContext(DbContextOptions<PayFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customers> Customers => Set<Customers>();

    public DbSet<Payments> Payments => Set<Payments>();

    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customers>(entity =>
        {
            entity.ToTable("Customers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.Document)
                .HasMaxLength(20)
                .IsRequired();

            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.HasIndex(x => x.Document)
                .IsUnique();

            entity.HasMany(x => x.Payments)
                .WithOne(x => x.Customers)
                .HasForeignKey(x => x.CustomerId);
        });

        modelBuilder.Entity<Payments>(entity =>
        {
            entity.ToTable("Payments");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(500);

            entity.HasOne(x => x.Customers)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.CustomerId);

            entity.HasOne(x => x.PaymentMethod)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.PaymentMethodId);
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.ToTable("PaymentMethods");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(150);

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });
    }
}