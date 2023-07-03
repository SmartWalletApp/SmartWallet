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

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Coin> Coin { get; set; }
        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<BalanceHistory> BalanceHistory { get; set; }

        public override int SaveChanges()
        {
            // If a new Customer has been added, add one wallet per each coin.
            var addedCustomerEntry = ChangeTracker.Entries<Customer>()
                .FirstOrDefault(entry => entry.State == EntityState.Added);

            if (addedCustomerEntry != null)
                return CreateWalletForNewCustomer(addedCustomerEntry.Entity);
            else
                return base.SaveChanges();
        }

        private int CreateWalletForNewCustomer(Customer customer)
        {
            using (var transaction = Database.BeginTransaction())
            {
                customer.Wallets = new List<Wallet>();
                var coins = Coin.ToList();
                foreach (var coin in coins)
                {
                    customer.Wallets.Add
                    (
                        new Wallet
                        {
                            Coin = coin,
                            Balance = 0,
                            BalanceHistory = new List<BalanceHistory>()
                        }
                    );
                }

                var modifiedRows = base.SaveChanges();
                transaction.Commit();
                return modifiedRows;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(b => b.Name).IsRequired();
                entity.Property(b => b.Surname).IsRequired();
                entity.Property(b => b.Email).IsRequired();
                entity.Property(b => b.Password).IsRequired();
                entity.Property(b => b.IsActive).IsRequired();
                entity.Property(b => b.CreationDate).IsRequired();
                //entity.Property(b => b.Wallets).IsRequired(); // Objects cannot be obligatory

                entity.HasMany(c => c.Wallets) // One customer has many wallets
                .WithOne() // Each wallet has one customer
                .HasForeignKey("CustomerId")
                .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Id).ValueGeneratedOnAdd();
                entity.Property(w => w.Balance).IsRequired();
                //entity.Property(w => w.Coin).IsRequired(); // Objects cannot be obligatory
                //entity.Property(w => w.BalanceHistory).IsRequired(); // Objects cannot be obligatory

                entity.HasOne(w => w.Coin);

                entity.HasMany(w => w.BalanceHistory) // One wallet has many BalanceHistories
                    .WithOne() // Each BalanceHistory has one Wallet
                    .HasForeignKey("WalletId")
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<BalanceHistory>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Id).ValueGeneratedOnAdd();
                entity.Property(b => b.Variation).IsRequired();
                entity.Property(b => b.Category).IsRequired();
                entity.Property(b => b.Date).IsRequired();
                entity.Property(b => b.Description).IsRequired();
            });

            modelBuilder.Entity<Coin>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.Property(b => b.Name).IsRequired();
                entity.Property(b => b.BuyValue).IsRequired();
                entity.Property(b => b.SellValue).IsRequired();
            });
        }

    }
}