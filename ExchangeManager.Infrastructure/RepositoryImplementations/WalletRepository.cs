
using ExchangeManager.DomainModel.Persistence;
using ExchangeManager.Infrastructure.DataModels;
using ExchangeManager.Infrastructure.Persistence;
using ExchangeManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExchangeManager.Infrastructure.RepositoryImplementations
{
    public class WalletRepository : ExchangeRepository<Wallet>, IWalletRepository<Wallet>
    {
        public WalletRepository(ExchangeManagerDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Wallet>> GetAll()
        {
            return await EntitySet.Include(w => w.Coin).ToListAsync();
        }

        public override async Task<Wallet> GetByID(int Id)
        {
            return await EntitySet.Include(w => w.Coin).FirstOrDefaultAsync(c => c.Id == Id);
        }
    }
}
