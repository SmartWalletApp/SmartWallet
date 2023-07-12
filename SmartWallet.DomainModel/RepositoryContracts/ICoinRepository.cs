namespace SmartWallet.DomainModel.RepositoryContracts
{
    public interface ICoinRepository<T> : ISmartWalletRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetByName(string name);
    }
}
