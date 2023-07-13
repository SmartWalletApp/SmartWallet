namespace SmartWallet.ApplicationService.Dto.Response
{
    public class CoinResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BuyValue { get; set; }
        public decimal SellValue { get; set; }
    }
}
