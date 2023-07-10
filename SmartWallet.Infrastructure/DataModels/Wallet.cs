namespace SmartWallet.Infrastructure.DataModels
{
    public class Wallet
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public Coin Coin { get; set; } = new Coin();
        public List<BalanceHistory> BalanceHistory { get; set; } = new List<BalanceHistory>();
        public int CustomerId { get; set; }
        public int CoinId { get; set; }
    }
}
