using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeManager.Infrastructure.DataModels
{
    public class Wallet
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public Coin Coin { get; set; } = new Coin();
        public List<BalanceHistory> BalanceHistory { get; set; } = new List<BalanceHistory>();
    }
}
