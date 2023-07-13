using SmartWallet.Infrastructure.DataModels;
using SmartWallet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SmartWallet.DomainModel.RepositoryContracts;
using SmartWallet.DomainModel.Entities.Response;

namespace SmartWallet.Infrastructure.RepositoryImplementations
{
    public class WalletRepository : SmartWalletRepository<Wallet>, IWalletRepository<Wallet>
    {
        public WalletRepository(SmartWalletDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Dictionary<string, KeyValuePair<decimal, List<BalanceHistoricResponseEntity>>>> GetBalanceHistorics(int customerId, string coinName, DateTime minDate, DateTime maxDate)
        {

            var balanceHistorics = await EntitySet
                .Where(w => w.CustomerId == customerId && w.Coin.Name == coinName)
                .Include(w => w.Coin)
                .Include(w => w.BalanceHistorics)
                .SelectMany(w => w.BalanceHistorics)
                .Where(bh => bh.Date > minDate && bh.Date < maxDate)
                .ToListAsync();

            var mappedBalanceHistorics =
                balanceHistorics.Select(bh => _mapper.Map<BalanceHistoricResponseEntity>(bh))
                .GroupBy(bh => bh.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    BalanceHistorics = new KeyValuePair<decimal, List<BalanceHistoricResponseEntity>>(
                        g.Sum(bh => bh.IsIncome ? bh.Variation : -bh.Variation),
                        g.ToList())
                })
                .ToDictionary(g => g.Category, g => g.BalanceHistorics);


            return mappedBalanceHistorics;

        }


    }
}
