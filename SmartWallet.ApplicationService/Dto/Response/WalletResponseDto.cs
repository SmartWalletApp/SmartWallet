namespace SmartWallet.ApplicationService.Dto.Response
{
    public class WalletResponseDto
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public CoinResponseDto Coin { get; set; } = new CoinResponseDto();
        public List<BalanceHistoryResponseDto> BalanceHistory { get; set; } = new List<BalanceHistoryResponseDto>();
    }
}
