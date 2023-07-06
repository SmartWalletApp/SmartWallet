namespace SmartWallet.ApplicationService.Dto.Response
{
    public class BalanceHistoryResponseDto
    {
        public decimal Variation { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
