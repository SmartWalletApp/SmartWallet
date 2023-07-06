
using SmartWallet.DomainModel.Persistence;
using SmartWallet.Infrastructure.DataModels;
using SmartWallet.Infrastructure.Persistence;
using SmartWallet.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace SmartWallet.Infrastructure.RepositoryImplementations
{
    public class CustomerRepository : SmartWalletRepository<Customer>, ICustomerRepository<Customer>
    {
        public CustomerRepository(SmartWalletDbContext context) : base(context)
        {
        }

        public async Task<Customer> GetByEmail(string email)
        {
            return await EntitySet.FirstAsync(x => x.Email == email);
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
