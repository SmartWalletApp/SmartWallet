namespace SmartWallet.ApplicationService.Dto.Request
{
    public class WalletRequestDto
    {
        public decimal Balance { get; set; }
        public CoinRequestDto Coin { get; set; } = new CoinRequestDto();
    }
}
