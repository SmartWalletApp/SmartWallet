using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeManager.DomainModel.RepositoryContracts
{
    public interface IExchangeRepository<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByID(int Id);
        Task<T> Insert(T entity);
        Task<T> Delete(int ID);
        Task Update(T entity);
        //void Save();
    }

}
