using SmartWallet.API.JWT.Implementations;
using System.Security.Cryptography;

namespace SmartWallet.API.StartUpConfigurations
{
    public class JWTExtensionConfigurator
    {
        public static JwtProperties SetJWTProperties()
        {
            string jwtKey = GenerateJWTKey();
            return new JwtProperties { Key = jwtKey, Issuer = "SmartWallet" };
        }

        private static string GenerateJWTKey()
        {
            var key = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return Convert.ToBase64String(key);
        }
    }
}
