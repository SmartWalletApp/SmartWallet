namespace SmartWallet.ApplicationService.Dto.Request
{
    public class BalanceHistoryRequestDto
    {
        public decimal Variation { get; set; }
        public bool IsIncome { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}
