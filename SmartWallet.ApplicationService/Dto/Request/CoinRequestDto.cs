namespace SmartWallet.ApplicationService.Dto.Request
{
    public class CoinRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal BuyValue { get; set; }
        public decimal SellValue { get; set; }
    }
}
