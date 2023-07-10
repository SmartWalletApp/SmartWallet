using Microsoft.EntityFrameworkCore;
using SmartWallet.Infrastructure.DataModels;

namespace SmartWallet.Infrastructure.Persistence
{
    public class SmartWalletDbContext : DbContext
    {
        public SmartWalletDbContext(DbContextOptions<SmartWalletDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Coin> Coin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Id).ValueGeneratedOnAdd();
                entity.Property(w => w.Balance).HasDefaultValue(0m);
                entity.Property(w => w.CoinId).IsRequired();
                entity.Property(w => w.CustomerId).IsRequired();

                entity.HasOne(w => w.Coin)
                      .WithMany()
                      .HasForeignKey(w => w.CoinId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(w => w.BalanceHistory)
                      .WithOne()
                      .HasForeignKey(bh => bh.WalletId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(w => new { w.CustomerId, w.CoinId }).IsUnique();
            });

            modelBuilder.Entity<BalanceHistory>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Id).ValueGeneratedOnAdd();
                entity.Property(b => b.Variation).IsRequired();
                entity.Property(b => b.IsIncome).IsRequired();
                entity.Property(b => b.Category).IsRequired();
                entity.Property(b => b.Date).HasDefaultValue(DateTime.Now);
                entity.Property(b => b.Description).IsRequired();
                entity.Property(b => b.WalletId).IsRequired();

                entity.HasOne(b => b.Wallet)
                      .WithMany(w => w.BalanceHistory)
                      .HasForeignKey(b => b.WalletId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Coin>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.BuyValue).IsRequired();
                entity.Property(c => c.SellValue).IsRequired();

                entity.HasIndex(c => c.Name).IsUnique();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.Surname).IsRequired();
                entity.Property(c => c.Email).IsRequired();
                entity.Property(c => c.Password).IsRequired();
                entity.Property(c => c.IsActive).HasDefaultValue(true);
                entity.Property(c => c.CreationDate).HasDefaultValue(DateTime.Now);
                entity.Property(c => c.SecurityGroup).HasDefaultValue("user");

                entity.HasIndex(c => c.Email).IsUnique();

                entity.HasMany(c => c.Wallets)
                      .WithOne()
                      .HasForeignKey(w => w.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }


    }
}