using SmartWallet.Infrastructure.DataModels;
using SmartWallet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SmartWallet.DomainModel.RepositoryContracts;
using SmartWallet.DomainModel.Entities.Response;
using Castle.Core.Resource;

namespace SmartWallet.Infrastructure.RepositoryImplementations
{
    public class WalletRepository : SmartWalletRepository<Wallet>, IWalletRepository<Wallet>
    {
        public WalletRepository(SmartWalletDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Dictionary<string, List<BalanceHistoricResponseEntity>>> GetBalanceHistorics(int customerId, string coinName, DateTime minDate, DateTime maxDate) =>
            await EntitySet
                .Include(w => w.Coin)
                .Include(w => w.BalanceHistorics)
                .Where(w => w.CustomerId == customerId && w.Coin.Name == coinName)
                .SelectMany(w => w.BalanceHistorics)
                .Where(bh => bh.Date > minDate && bh.Date < maxDate)
                .Select(bh => _mapper.Map<BalanceHistoricResponseEntity>(bh))
                .GroupBy(bh => bh.Category)
                .ToDictionaryAsync(g => g.Key, g => g.ToList());

        public async Task<IEnumerable<WalletResponseEntity>> GetWallets(int customerId) =>
            await EntitySet
                .Include(w => w.Coin)
                .Include(w => w.BalanceHistorics)
                .Where(w => w.CustomerId == customerId)
                .Select(w => _mapper.Map<WalletResponseEntity>(w))
                .ToListAsync();

    }
}
