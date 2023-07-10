namespace SmartWallet.DomainModel.Dto.Request
{
    public class CoinRequestEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal BuyValue { get; set; }
        public decimal SellValue { get; set; }
    }
}
