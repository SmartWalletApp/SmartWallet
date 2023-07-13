using SmartWallet.DomainModel.RepositoryContracts;
using SmartWallet.Infrastructure.DataModels;

namespace SmartWallet.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartWalletDbContext _context;

        public ICustomerRepository<Customer> CustomerRepository { get; }
        public ICoinRepository<Coin> CoinRepository { get; }
        public IWalletRepository<Wallet> WalletRepository { get; }
        public IBalanceHistoric<BalanceHistoric> BalanceHistoricRepository { get; }

        public UnitOfWork(
            SmartWalletDbContext context,
            ICustomerRepository<Customer> customerRepository,
            ICoinRepository<Coin> coinRepository,
            IWalletRepository<Wallet> walletRepository,
            IBalanceHistoric<BalanceHistoric> balanceHistoricRepository)
        {
            _context = context;
            CustomerRepository = customerRepository;
            CoinRepository = coinRepository;
            WalletRepository = walletRepository;
            BalanceHistoricRepository = balanceHistoricRepository;
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