using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeManager.Infrastructure.DataModels
{
    public class BalanceHistory
    {
        public int Id { get; set; }
        public decimal Variation { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Wallet Wallet { get; set; }
    }
}
