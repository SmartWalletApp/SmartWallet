namespace SmartWallet.ApplicationService.Dto.Request
{
    public class BalanceHistoryRequestDto
    {
        public decimal Variation { get; set; }
        public bool IsIncome { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
