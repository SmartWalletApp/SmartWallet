namespace SmartWallet.DomainModel.Dto.Request
{
    public class CustomerRequestEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SecurityGroup { get; set; } = "user";
    }
}
