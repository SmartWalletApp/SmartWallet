namespace SmartWallet.Infrastructure.DataModels
{
    public class Wallet
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public Coin? Coin { get; set; }
        public List<BalanceHistory>? BalanceHistory { get; set; }
    }
}
