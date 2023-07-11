using SmartWallet.DomainModel.Entities.Response;

namespace SmartWallet.DomainModel.RepositoryContracts
{
    public interface IWalletRepository<T> : ISmartWalletRepository<T> where T : class
    {
        public Task<Dictionary<string, List<BalanceHistoricResponseEntity>>> GetBalanceHistorics(int customerId, string coinName, DateTime minDate, DateTime maxDate);

        public Task<IEnumerable<WalletResponseEntity>> GetWallets(int customerId);
    }
}