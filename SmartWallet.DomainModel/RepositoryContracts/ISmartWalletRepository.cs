namespace SmartWallet.DomainModel.RepositoryContracts
{
    public interface ISmartWalletRepository<T> : IDisposable where T : class
    {
        Task<T> Insert(T entity);
        Task<T> Delete(int ID);
    }

}
