namespace SmartWallet.DomainModel.Dto.Request
{
    public class CoinRequestEntity
    {
        public string? Name { get; set; }
        public decimal BuyValue { get; set; }
        public decimal SellValue { get; set; }
    }
}
