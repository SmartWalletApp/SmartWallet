namespace SmartWallet.DomainModel.Dto.Request
{
    public class WalletRequestEntity
    {
        public decimal Balance { get; set; }
        public CoinRequestEntity Coin { get; set; } = new CoinRequestEntity();
    }
}
