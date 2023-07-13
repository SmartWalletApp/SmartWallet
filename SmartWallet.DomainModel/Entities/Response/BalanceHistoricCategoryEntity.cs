using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWallet.DomainModel.Entities.Response
{
    public class BalanceHistoricCategoryEntity<TKey, TValue>
    {
        public TKey Total { get; set; }
        public TValue BalanceHistorics { get; set; }

        public BalanceHistoricCategoryEntity(TKey key, TValue value)
        {
            Total = key;
            BalanceHistorics = value;
        }
    }

}
