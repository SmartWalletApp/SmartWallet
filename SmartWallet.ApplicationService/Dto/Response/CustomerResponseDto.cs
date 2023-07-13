namespace SmartWallet.ApplicationService.Dto.Response
{
    public class CustomerResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public string SecurityGroup { get; set; } = string.Empty;
        public List<WalletResponseDto> Wallets { get; set; } = new List<WalletResponseDto>();
    }
}
