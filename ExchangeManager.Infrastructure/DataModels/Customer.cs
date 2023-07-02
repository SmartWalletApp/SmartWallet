using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeManager.Infrastructure.DataModels
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Default";
        public string Surname { get; set; } = "Default";
        public string Email { get; set; } = "Default";
        public string Password { get; set; } = "Default";
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Wallet> Wallets { get; set; } = new List<Wallet>();
    }
}
