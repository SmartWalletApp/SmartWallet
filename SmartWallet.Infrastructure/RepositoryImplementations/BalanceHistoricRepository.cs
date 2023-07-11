using SmartWallet.Infrastructure.DataModels;
using SmartWallet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SmartWallet.DomainModel.RepositoryContracts;

namespace SmartWallet.Infrastructure.RepositoryImplementations
{
    public class BalanceHistoricRepository : SmartWalletRepository<BalanceHistoric>, IBalanceHistoric<BalanceHistoric>
    {
        public BalanceHistoricRepository(SmartWalletDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

    }
}
