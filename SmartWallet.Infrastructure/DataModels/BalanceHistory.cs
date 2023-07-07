namespace SmartWallet.Infrastructure.DataModels
{
    public class BalanceHistory
    {
        public int Id { get; set; }
        public decimal Variation { get; set; }
        public bool IsIncome { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
