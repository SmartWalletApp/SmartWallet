using SmartWallet.DomainModel.Entities.Response;

namespace SmartWallet.ApplicationService.Dto.Response
{
    public class CustomerResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public string? SecurityGroup { get; set; }
        public List<WalletResponseEntity>? Wallets { get; set; }
    }
}
