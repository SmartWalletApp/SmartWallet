namespace SmartWallet.DomainModel.Entities
{
    public class Coin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal BuyValue { get; set; }
        public decimal SellValue { get; set; }
    }
}
