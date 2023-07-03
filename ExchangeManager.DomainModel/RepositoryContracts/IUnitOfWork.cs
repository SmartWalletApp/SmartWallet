namespace ExchangeManager.DomainModel.RepositoryContracts
{
    public interface IUnitOfWork
    {
        void Save();
        void EnsureDeleted();
        void EnsureCreated();
    }
}
