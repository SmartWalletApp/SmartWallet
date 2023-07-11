namespace SmartWallet.DomainModel.Dto.Request
{
    public class BalanceHistoricRequestEntity
    {
        public decimal Variation { get; set; }
        public bool IsIncome { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
