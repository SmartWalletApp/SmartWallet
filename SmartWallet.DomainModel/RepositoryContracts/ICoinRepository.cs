namespace SmartWallet.DomainModel.RepositoryContracts
{
    public interface ICoinRepository<T> : ISmartWalletRepository<T> where T : class
    {
        public Task<T> GetByName(string name);
    }
}
