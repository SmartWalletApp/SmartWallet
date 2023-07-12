using SmartWallet.Infrastructure.DataModels;

namespace SmartWallet.ApplicationService.Dto.Response
{
    public class WalletResponseDto
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public CoinResponseDto Coin { get; set; } = new CoinResponseDto();
        public List<BalanceHistoric> BalanceHistorics { get; set; } = new List<BalanceHistoric>();
    }
}
