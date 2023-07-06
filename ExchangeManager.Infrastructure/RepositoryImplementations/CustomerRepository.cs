
using ExchangeManager.DomainModel.Persistence;
using ExchangeManager.Infrastructure.DataModels;
using ExchangeManager.Infrastructure.Persistence;
using ExchangeManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace ExchangeManager.Infrastructure.RepositoryImplementations
{
    public class CustomerRepository : ExchangeRepository<Customer>, ICustomerRepository<Customer>
    {
        public CustomerRepository(ExchangeManagerDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Customer>> GetAll()
        {
            return await EntitySet.Include(c => c.Wallets)
                .ThenInclude(w => w.Coin)
                .Include(c => c.Wallets)
                .ThenInclude(w => w.BalanceHistory)
                .ToListAsync();
        }

        public override async Task<Customer> GetByID(int Id)
        {
            return await EntitySet.Include(c => c.Wallets)
                .ThenInclude(w => w.Coin)
                .Include(c => c.Wallets)
                .ThenInclude(w => w.BalanceHistory)
                .FirstOrDefaultAsync(c => c.Id == Id);
        }
    }
}
