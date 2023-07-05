using ExchangeManager.DomainModel.Persistence;
using ExchangeManager.DomainModel.RepositoryContracts;
using ExchangeManager.Infrastructure.DataModels;

namespace ExchangeManager.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExchangeManagerDbContext _context;

        public ICustomerRepository<Customer> CustomerRepository { get; }
        public IWalletRepository<Wallet> WalletRepository { get; }
        public IExchangeRepository<BalanceHistory> BalanceHistoryRepository { get; }
        public IExchangeRepository<Coin> CoinRepository { get; }

        public UnitOfWork(
            ExchangeManagerDbContext context,
            ICustomerRepository<Customer> customerRepository,
            IWalletRepository<Wallet> walletRepository,
            IExchangeRepository<Coin> coinRepository,
            IExchangeRepository<BalanceHistory> balanceHistoryRepository)
        {
            _context = context;
            CustomerRepository = customerRepository;
            WalletRepository = walletRepository;
            CoinRepository = coinRepository;
            BalanceHistoryRepository = balanceHistoryRepository;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void EnsureDeleted()
        {
            _context.Database.EnsureDeleted();
        }

        public void EnsureCreated()
        {
            _context.Database.EnsureCreated();
        }
    }
}
