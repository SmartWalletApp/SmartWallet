using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeManager.Infrastructure.DataModels
{
    public class Coin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal BuyValue { get; set; }
        public decimal SellValue { get; set; }
        public List<Wallet> Wallets { get; set; }
    }
}
