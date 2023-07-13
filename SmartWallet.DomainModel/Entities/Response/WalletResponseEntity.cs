namespace SmartWallet.DomainModel.Entities.Response
{
    public class WalletResponseEntity
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public CoinResponseEntity Coin { get; set; } = new CoinResponseEntity();
        public List<BalanceHistoricResponseEntity> BalanceHistorics { get; set; } = new List<BalanceHistoricResponseEntity>();
    }
}
