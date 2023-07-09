using SmartWallet.DomainModel.RepositoryContracts;
using SmartWallet.Infrastructure.DataModels;

namespace SmartWallet.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartWalletDbContext _context;

        public ICustomerRepository<Customer> CustomerRepository { get; }
        public ICoinRepository<Coin> CoinRepository { get; }

        public UnitOfWork(
            SmartWalletDbContext context,
            ICustomerRepository<Customer> customerRepository,
            ICoinRepository<Coin> coinRepository)
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
