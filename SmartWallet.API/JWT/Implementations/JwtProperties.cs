using SmartWallet.API.JWT.Contracts;

namespace SmartWallet.API.JWT.Implementations
{
    public class JwtProperties : IJwtProperties
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
    }
}
