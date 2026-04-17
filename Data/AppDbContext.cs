
using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Models;
using System.Reflection.Emit;

namespace PharmacyAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Pharmacist> Pharmacists { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── User ──────────────────────────────────────────────────
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(150);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Role).HasDefaultValue("Customer");
            });

            // ── Pharmacist ───────────────────────────────────────────
            modelBuilder.Entity<Pharmacist>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.LicenseNumber).IsUnique();
                entity.Property(p => p.LicenseNumber).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Status).HasDefaultValue("Pending");
                entity.HasOne(p => p.User)
                      .WithOne(u => u.Pharmacist)
                      .HasForeignKey<Pharmacist>(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Category ─────────────────────────────────────────────
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            // ── Medicine ─────────────────────────────────────────────
            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Name).IsRequired().HasMaxLength(200);
                entity.Property(m => m.Price).HasPrecision(10, 2);
                entity.HasOne(m => m.Category)
                      .WithMany(c => c.Medicines)
                      .HasForeignKey(m => m.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Inventory ────────────────────────────────────────────
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.HasOne(i => i.Medicine)
                      .WithOne(m => m.Inventory)
                      .HasForeignKey<Inventory>(i => i.MedicineId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Prescription ─────────────────────────────────────────
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FilePath).IsRequired();
                entity.HasOne(p => p.User)
                      .WithMany(u => u.Prescriptions)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Order ────────────────────────────────────────────────
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.TotalAmount).HasPrecision(12, 2);
                entity.Property(o => o.Status).HasDefaultValue("Pending");
                entity.HasOne(o => o.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.Prescription)
                      .WithMany(p => p.Orders)
                      .HasForeignKey(o => o.PrescriptionId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // ── OrderItem ────────────────────────────────────────────
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.UnitPrice).HasPrecision(10, 2);
                entity.Property(oi => oi.TotalPrice).HasPrecision(10, 2);
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(oi => oi.Medicine)
                      .WithMany(m => m.OrderItems)
                      .HasForeignKey(oi => oi.MedicineId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── EmailVerification ────────────────────────────────────
            modelBuilder.Entity<EmailVerification>(entity =>
            {
                entity.HasKey(ev => ev.Id);
                entity.HasOne(ev => ev.User)
                      .WithMany(u => u.EmailVerifications)
                      .HasForeignKey(ev => ev.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ── LoginHistory ─────────────────────────────────────────
            modelBuilder.Entity<LoginHistory>(entity =>
            {
                entity.HasKey(lh => lh.Id);
                entity.HasOne(lh => lh.User)
                      .WithMany(u => u.LoginHistories)
                      .HasForeignKey(lh => lh.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
