using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeManager.DomainModel.RepositoryContracts
{
    public interface IUnitOfWork
    {
        void Save();
        void EnsureDeleted();
        void EnsureCreated();
        void SetCoins();
    }
}
