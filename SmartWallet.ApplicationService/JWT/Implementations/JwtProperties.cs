using SmartWallet.ApplicationService.JWT.Contracts;

namespace SmartWallet.API.JWT.Implementations
{
    public class JwtProperties : IJwtProperties
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
    }
}
