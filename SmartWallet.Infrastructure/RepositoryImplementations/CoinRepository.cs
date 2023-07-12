using SmartWallet.Infrastructure.DataModels;
using SmartWallet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SmartWallet.DomainModel.RepositoryContracts;

namespace SmartWallet.Infrastructure.RepositoryImplementations
{
    public class CoinRepository : SmartWalletRepository<Coin>, ICoinRepository<Coin>
    {
        public CoinRepository(SmartWalletDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<Coin>> GetAll()
        {
            return await EntitySet.ToListAsync();
        }
        public async Task<Coin> GetByName(string name)
        {
            return await EntitySet.FirstOrDefaultAsync(x => x.Name == name) ?? throw new InvalidOperationException("Customer not found");
        }
    }
}
