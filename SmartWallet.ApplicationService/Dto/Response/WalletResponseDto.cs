namespace SmartWallet.ApplicationService.Dto.Response
{
    public class WalletResponseDto
    {
        public decimal Balance { get; set; }
        public CoinResponseDto Coin { get; set; }
    }
}
