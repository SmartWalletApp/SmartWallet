using ExchangeManager.DomainModel.RepositoryContracts;

namespace ExchangeManager.DomainModel.Persistence
{
    public interface ICustomerRepository<T> : IExchangeRepository<T> where T : class
    {
    }
}