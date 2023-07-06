namespace SmartWallet.DomainModel.Entities
{
    public class BalanceHistory
    {
        public int Id { get; set; }
        public decimal Variation { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
