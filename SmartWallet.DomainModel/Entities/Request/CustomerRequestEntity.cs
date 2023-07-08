namespace SmartWallet.DomainModel.Dto.Request
{
    public class CustomerRequestEntity
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? SecurityGroup { get; set; }
    }
}
