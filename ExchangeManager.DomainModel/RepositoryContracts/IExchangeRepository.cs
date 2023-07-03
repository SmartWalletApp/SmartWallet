namespace ExchangeManager.DomainModel.RepositoryContracts
{
    public interface IExchangeRepository<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByID(int Id);
        Task<T> Insert(T entity);
        Task<T> Delete(int ID);
        Task<T> Update(T entity);
    }

}
