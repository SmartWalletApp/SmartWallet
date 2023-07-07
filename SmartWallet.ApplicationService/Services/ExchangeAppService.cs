using SmartWallet.ApplicationService.Contracts;
using SmartWallet.Infrastructure.DataModels;
using SmartWallet.DomainModel.RepositoryContracts;
using System.Text;
using SmartWallet.Infrastructure.Persistence;
using Microsoft.IdentityModel.Tokens;
using SmartWallet.ApplicationService.JWT.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartWallet.ApplicationService.Services
{
    public class SmartWalletAppService : ISmartWalletAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProperties _jwtProperties;

        public SmartWalletAppService(IUnitOfWork unitOfWork, IJwtProperties jwtProperties)
        {
            _unitOfWork = unitOfWork;
            _jwtProperties = jwtProperties;
        }

        public async Task<Customer> VerifyCustomerLogin(string givenEmail, string givenPassword)
        {
            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByEmail(givenEmail);
            if (customerFound != null)
            {
                if (VerifyPassword(givenPassword, customerFound.Password))
                {
                    return customerFound;
                }
            }
            return null;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await ((UnitOfWork)_unitOfWork).CustomerRepository.GetAll();
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(id);
        }

        public async Task<Customer> InsertCustomer(Customer customer)
        {
            // Currently bcrypt will use Cost 11 (2048 iteratios) by default, which is around 140ms in debug using a Ryzen 5 3600X and DDR4 3200MHz RAM.
            customer.Password = HashPassword(customer.Password);

            var coins = await ((UnitOfWork)_unitOfWork).CoinRepository.GetAll();

            customer.Wallets = new List<Wallet>();

            foreach (var coin in coins)
            {
                var wallet = new Wallet
                {
                    Balance = 0,
                    Coin = coin,
                    BalanceHistory = new List<BalanceHistory>()
                };
                customer.Wallets.Add(wallet);
            }
            var output = await ((UnitOfWork)_unitOfWork).CustomerRepository.Insert(customer);
            _unitOfWork.Save();
            return output;
        }

        public async Task<Customer> DeleteCustomer(int id)
        {
            var output = await ((UnitOfWork)_unitOfWork).CustomerRepository.Delete(id);
            _unitOfWork.Save();
            return output;
        }

        public async Task<Customer> UpdateCustomer(Customer newCustomer)
        {
            var output = await ((UnitOfWork)_unitOfWork).CustomerRepository.Update(newCustomer);
            _unitOfWork.Save();
            return output;
        }

        public Task RestoreDB()
        {
            _unitOfWork.EnsureDeleted();
            _unitOfWork.EnsureCreated();

            #if DEBUG
            var debugUser = InsertCustomer(new Customer
            {
                Name = "debug",
                Surname = "debug",
                Email = "debug",
                Password = "debug",
                IsActive = true
            }).Result;
            #endif

            ((UnitOfWork)_unitOfWork).CoinRepository.Insert(new Coin { Name = "USD", BuyValue = 0, SellValue = 0 });
            ((UnitOfWork)_unitOfWork).CoinRepository.Insert(new Coin { Name = "EUR", BuyValue = 0, SellValue = 0 });
            ((UnitOfWork)_unitOfWork).CoinRepository.Insert(new Coin { Name = "BTC", BuyValue = 0, SellValue = 0 });
            
            _unitOfWork.Save();
            return Task.CompletedTask;
        }

        private string HashPassword(string unhashedPass)
        {
            // Currently bcrypt will use Cost 11 (2048 iteratios) by default, which is around 140ms in debug using a Ryzen 5 3600X and DDR4 3200MHz RAM.
            return BCrypt.Net.BCrypt.HashPassword(unhashedPass);
        }
        private bool VerifyPassword(string unhashedText, string hashedText)
        {
            return BCrypt.Net.BCrypt.Verify(unhashedText, hashedText);
        }

        public bool CheckJwtAuthentication(string? jwtToken)
        {
            var key = Encoding.UTF8.GetBytes(_jwtProperties.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidIssuer = _jwtProperties.Issuer,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(jwtToken, validationParameters, out _);

            return true;
        }

        public string GetTokenString(Customer validatedCustomer)
        {
            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, validatedCustomer.Email)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtProperties.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _jwtProperties.Issuer
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}