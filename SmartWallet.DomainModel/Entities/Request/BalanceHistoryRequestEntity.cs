namespace SmartWallet.DomainModel.Dto.Request
{
    public class BalanceHistoryRequestEntity
    {
        public decimal Variation { get; set; }
        public bool IsIncome { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
