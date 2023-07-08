namespace SmartWallet.DomainModel.Entities.Response
{
    public class CoinResponseEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal BuyValue { get; set; }
        public decimal SellValue { get; set; }
    }
}
