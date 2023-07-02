using ExchangeManager.Data;
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

        public ExchangeRepository<Customer> CustomerRepository;
        public ExchangeRepository<Coin> CoinRepository;
        public ExchangeRepository<BalanceHistory> BalanceHistoryRepository;
        public ExchangeRepository<Wallet> WalletRepository;

        public void Save() { 
            _context.SaveChanges();
        }

      
    }
}
