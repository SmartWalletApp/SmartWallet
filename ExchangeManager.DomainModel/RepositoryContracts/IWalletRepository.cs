using ExchangeManager.DomainModel.RepositoryContracts;

namespace ExchangeManager.DomainModel.Persistence
{
    public interface IWalletRepository<T> : IExchangeRepository<T> where T : class
    {
    }
}