using ExchangeManager.DomainModel.RepositoryContracts;
using ExchangeManager.Infrastructure.DataModels;
using ExchangeManager.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeManager.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExchangeManagerDbContext _context;

        public IExchangeRepository<Customer> CustomerRepository;
        public IExchangeRepository<Coin> CoinRepository;
        public IExchangeRepository<BalanceHistory> BalanceHistoryRepository;
        public IExchangeRepository<Wallet> WalletRepository;

        public UnitOfWork(ExchangeManagerDbContext context,
            IExchangeRepository<Customer> customerRepository,
            IExchangeRepository<Coin> coinRepository,
            IExchangeRepository<BalanceHistory> balanceHistoryRepository,
            IExchangeRepository<Wallet> walletRepository)
        {
            _context = context;
            CustomerRepository = customerRepository;
            CoinRepository = coinRepository;
            BalanceHistoryRepository = balanceHistoryRepository;
            WalletRepository = walletRepository;
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

        public void SetCoins()
        {
            CoinRepository.Insert(new Coin
            {
                Name = "Euro",
                SellValue = 1,
                BuyValue = 1
            });
            CoinRepository.Insert(new Coin
            {
                Name = "Dollar",
                SellValue = 1.2m,
                BuyValue = 0.8m
            });
            CoinRepository.Insert(new Coin
            {
                Name = "Bitcoin",
                SellValue = 20000m,
                BuyValue = 22000m
            });
            CoinRepository.Insert(new Coin
            {
                Name = "DogeCoin",
                SellValue = 0.000003m,
                BuyValue = 0.0000013m
            });
        }


    }
}
