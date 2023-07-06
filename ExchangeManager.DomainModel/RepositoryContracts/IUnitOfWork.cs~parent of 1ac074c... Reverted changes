using ExchangeManager.DomainModel.DataModels;

namespace ExchangeManager.DomainModel.RepositoryContracts
{
    public interface IUnitOfWork
    {
        IExchangeRepository<Customer> CustomerRepository { get; }
        IExchangeRepository<Coin> CoinRepository { get; }

        void Save();
        void EnsureDeleted();
        void EnsureCreated();
    }
}
