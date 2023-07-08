using SmartWallet.ApplicationService.Contracts;
using SmartWallet.Infrastructure.DataModels;
using SmartWallet.DomainModel.RepositoryContracts;
using System.Text;
using SmartWallet.Infrastructure.Persistence;
using Microsoft.IdentityModel.Tokens;
using SmartWallet.ApplicationService.JWT.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SmartWallet.ApplicationService.Dto.Response;
using AutoMapper;
using SmartWallet.ApplicationService.Dto.Request;
using SmartWallet.DomainModel.Dto.Request;
using Castle.Core.Resource;
using SmartWallet.DomainModel.Entities.Response;

namespace SmartWallet.ApplicationService.Services
{
    public class SmartWalletAppService : ISmartWalletAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProperties _jwtProperties;
        private readonly IMapper _mapper;

        public SmartWalletAppService(IUnitOfWork unitOfWork, IJwtProperties jwtProperties, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtProperties = jwtProperties;
            _mapper = mapper;
        }

        public async Task<CustomerResponseDto> VerifyCustomerLogin(string givenEmail, string givenPassword)
        {
            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByEmail(givenEmail);
            if (customerFound != null)
            {
                if (customerFound.IsActive)
                {
                    if (VerifyPassword(givenPassword, customerFound.Password))
                    {
                        var customerFoundResponseEntity = _mapper.Map<CustomerResponseEntity>(customerFound);
                        var customerFoundResponseDto = _mapper.Map<CustomerResponseDto>(customerFoundResponseEntity);
                        return customerFoundResponseDto;
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetCustomers()
        {
            var customers = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetAll();
            var customerResponseEntities = customers.Select(customer => _mapper.Map<CustomerResponseEntity>(customer)).ToList();
            return customerResponseEntities.Select(customerResponseEntity => _mapper.Map<CustomerResponseDto>(customerResponseEntity)).ToList();
            
        }

        public async Task<CustomerResponseDto> GetCustomerById(int id)
        {
            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(id);
            var customerFoundResponseEntity = _mapper.Map<CustomerResponseEntity>(customerFound);
            var customerFoundResponseDto = _mapper.Map<CustomerResponseDto>(customerFoundResponseEntity);

            return customerFoundResponseDto;
        }

        public async Task<CustomerResponseDto> GetCustomerByEmail(string email)
        {
            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByEmail(email);
            var customerFoundResponseEntity = _mapper.Map<CustomerResponseEntity>(customerFound);
            var customerFoundResponseDto = _mapper.Map<CustomerResponseDto>(customerFoundResponseEntity);

            return customerFoundResponseDto;
        }

        public async Task<CustomerResponseDto> InsertCustomer(CustomerRequestDto newCustomer)
        {
            // Currently bcrypt will use Cost 11 (2048 iteratios) by default, which is around 140ms in debug using a Ryzen 5 3600X and DDR4 3200MHz RAM.
            newCustomer.Password = HashPassword(newCustomer.Password);

            var coins = await ((UnitOfWork)_unitOfWork).CoinRepository.GetAll();

            var newCustomerEntity = _mapper.Map<CustomerRequestEntity>(newCustomer);
            var customer = _mapper.Map<Customer>(newCustomerEntity);

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
            var customerInserted = await ((UnitOfWork)_unitOfWork).CustomerRepository.Insert(customer);
            _unitOfWork.Save();
            var customerInsertedResponseEntity = _mapper.Map<CustomerResponseEntity>(customerInserted);
            var customerInsertedResponseDto = _mapper.Map<CustomerResponseDto>(customerInsertedResponseEntity);
            return customerInsertedResponseDto;
        }

        public async Task<CustomerResponseDto> DeleteCustomer(int id)
        {
            var customerDeleted = await ((UnitOfWork)_unitOfWork).CustomerRepository.Delete(id);
            _unitOfWork.Save();
            var customerDeletedResponseEntity = _mapper.Map<CustomerResponseEntity>(customerDeleted);
            var customerDeletedResponseDto = _mapper.Map<CustomerResponseDto>(customerDeletedResponseEntity);

            return customerDeletedResponseDto;
        }

        public async Task<CustomerResponseDto> UpdateCustomer(CustomerRequestDto newCustomer)
        {
            var newCustomerEntity = _mapper.Map<CustomerRequestEntity>(newCustomer);
            var customer = _mapper.Map<Customer>(newCustomerEntity);
            customer.Password = HashPassword(newCustomer.Password);

            var customerUpdated = await ((UnitOfWork)_unitOfWork).CustomerRepository.Update(customer);
            _unitOfWork.Save();

            var customerUpdatedResponseEntity = _mapper.Map<CustomerResponseEntity>(customerUpdated);
            var customerUpdatedResponseDto = _mapper.Map<CustomerResponseDto>(customerUpdatedResponseEntity);

            return customerUpdatedResponseDto;
        }

        public Task RestoreDB()
        {
            _unitOfWork.EnsureDeleted();
            _unitOfWork.EnsureCreated();

            #if DEBUG
            var debugUser = InsertCustomer(new CustomerRequestDto
            {
                Name = "admin",
                Surname = "admin",
                Email = "admin",
                Password = "admin",
                SecurityGroup = "admin"
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

        public Dictionary<string, string> GetTokenInfo(string? jwtToken)
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
            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);
            }
            catch
            {
                return null;
            }


            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                return jwtSecurityToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            }
            return null;
        }

        public string CreateToken(CustomerResponseDto validatedCustomer)
        {
            var claims = new[]
            {
                new Claim("email", validatedCustomer.Email),
                new Claim("id", validatedCustomer.Id.ToString()),
                new Claim("group", validatedCustomer.SecurityGroup)
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