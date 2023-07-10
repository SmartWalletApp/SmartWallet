namespace SmartWallet.Infrastructure.DataModels
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public string SecurityGroup { get; set; } = string.Empty;
        public List<Wallet> Wallets { get; set; } = new List<Wallet>();
    }
}
