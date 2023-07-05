using ExchangeManager.DomainModel.RepositoryContracts;
using ExchangeManager.DomainModel.DataModels;

namespace ExchangeManager.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExchangeManagerDbContext _context;

        public IExchangeRepository<Customer> CustomerRepository { get; }
        public IExchangeRepository<Customer> WalletRepository { get; }
        public IExchangeRepository<Customer> BalanceHistoryRepository { get; }
        public IExchangeRepository<Coin> CoinRepository { get; }

        public UnitOfWork(
            ExchangeManagerDbContext context,
            IExchangeRepository<Customer> customerRepository,
            IExchangeRepository<Coin> coinRepository)
        {
            _context = context;
            CustomerRepository = customerRepository;
            CoinRepository = coinRepository;
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
