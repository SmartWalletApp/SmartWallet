namespace SmartWallet.DomainModel.Entities.Response
{
    public class CustomerResponseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public string SecurityGroup { get; set; }
        public List<WalletResponseEntity> Wallets { get; set; }
    }
}
