using Microsoft.EntityFrameworkCore;
using ExchangeManager.Infrastructure.DataModels;
using ExchangeManager.DomainModel.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace ExchangeManager.Infrastructure.Persistence
{
    public class ExchangeManagerDbContext : DbContext
    {
        public ExchangeManagerDbContext(DbContextOptions<ExchangeManagerDbContext> options) : base(options)
        {
        }

        // Customer will contain all other relations.
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Coin> Coin { get; set; }
        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<BalanceHistory> BalanceHistory { get; set; }

        public override int SaveChanges()
        {
            var addedCustomerEntry = ChangeTracker.Entries<Customer>()
                .FirstOrDefault(entry => entry.State == EntityState.Added);

            if (addedCustomerEntry != null)
            {
                using (var transaction = Database.BeginTransaction()){
                    try
                    {
                        int result = base.SaveChanges();

                        var addedCustomer = addedCustomerEntry.Entity;

                        var coins = Coin.ToList();

                        foreach (var coin in coins)
                        {
                            addedCustomer.Wallets.Add
                            (
                                new Wallet { Coin = coin}
                            );
                        }

                        base.SaveChanges();
                        transaction.Commit();

                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(b => b.CreationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Id).ValueGeneratedOnAdd();
                entity.Property(w => w.Balance).HasDefaultValue(0);
            });

            modelBuilder.Entity<BalanceHistory>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Id).ValueGeneratedOnAdd();
                entity.Property(b => b.Date).HasDefaultValueSql("CURRENT_TIMESTAMP");

            });

            modelBuilder.Entity<Coin>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
            });
        }

    }
}