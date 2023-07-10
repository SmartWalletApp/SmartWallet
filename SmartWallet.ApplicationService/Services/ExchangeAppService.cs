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

        public async Task<CustomerResponseDto> AddHistoric(int clientId, BalanceHistoryRequestDto historic, string coin)
        {

            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(clientId) ?? throw new Exception("Customer does not exist");

            var walletFound = customerFound.Wallets.Find(x => x.Coin.Name == coin) ?? throw new Exception("Wallet with that name does not exist");


            var newHistoricRequestEntity = _mapper.Map<BalanceHistoryRequestEntity>(historic);
            var newHistoric = _mapper.Map<BalanceHistory>(newHistoricRequestEntity);

            walletFound.BalanceHistory.Add(newHistoric);
            _unitOfWork.Save();

            var customerFoundResponseEntity = _mapper.Map<CustomerResponseEntity>(customerFound);
            var customerFoundResponseDto = _mapper.Map<CustomerResponseDto>(customerFoundResponseEntity);

            return customerFoundResponseDto;
        }

        public async Task<CustomerResponseDto> RemoveWallet(int clientId, string coin)
        {
            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(clientId);
            if (customerFound == null) throw new Exception("Customer does not exist");

            var walletsToRemove = customerFound.Wallets.Find(x => x.Coin.Name == coin);
            if (walletsToRemove == null) throw new Exception("No wallet with that coin found");

            customerFound.Wallets.Remove(walletsToRemove);

            _unitOfWork.Save();

            var customerFoundResponseEntity = _mapper.Map<CustomerResponseEntity>(customerFound);
            var customerFoundResponseDto = _mapper.Map<CustomerResponseDto>(customerFoundResponseEntity);

            return customerFoundResponseDto;
        }

        public Task<IEnumerable<Coin>> GetCoins()
        {
            return ((UnitOfWork)_unitOfWork).CoinRepository.GetAll();
        }

        public async Task<CustomerResponseDto> VerifyCustomerLogin(string givenEmail, string givenPassword)
        {
            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByEmail(givenEmail);
            if (customerFound == null) throw new Exception("Wrong user or password");
            if (!customerFound.IsActive) throw new Exception("Customer is disabled");

            if (VerifyPassword(givenPassword, customerFound.Password))
            {
                var customerFoundResponseEntity = _mapper.Map<CustomerResponseEntity>(customerFound);
                var customerFoundResponseDto = _mapper.Map<CustomerResponseDto>(customerFoundResponseEntity);
                return customerFoundResponseDto;
            } else
            {
                throw new Exception("Wrong user or password");
            }
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetCustomers()
        {
            var customers = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetAll();
            if (!customers.Any()) throw new Exception("No customers found");

            var customerResponseEntities = customers.Select(customer => _mapper.Map<CustomerResponseEntity>(customer)).ToList();
            return customerResponseEntities.Select(customerResponseEntity => _mapper.Map<CustomerResponseDto>(customerResponseEntity)).ToList();
        }

        public async Task<CustomerResponseDto> GetCustomerById(int id)
        {
            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(id);
            if (customerFound == null) throw new Exception("Customer does not exist");

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
            if (customerInserted == null) throw new Exception("Failed to insert customer");

            _unitOfWork.Save();
            var customerInsertedResponseEntity = _mapper.Map<CustomerResponseEntity>(customerInserted);
            var customerInsertedResponseDto = _mapper.Map<CustomerResponseDto>(customerInsertedResponseEntity);
            return customerInsertedResponseDto;
        }

        public async Task<CustomerResponseDto> DeleteCustomer(int id)
        {
            var customerDeleted = await ((UnitOfWork)_unitOfWork).CustomerRepository.Delete(id);
            if (customerDeleted == null) throw new Exception("Failed to delete customer");

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
            if (customerUpdated == null) throw new Exception("Failed to update customer");

            _unitOfWork.Save();

            var customerUpdatedResponseEntity = _mapper.Map<CustomerResponseEntity>(customerUpdated);
            var customerUpdatedResponseDto = _mapper.Map<CustomerResponseDto>(customerUpdatedResponseEntity);

            return customerUpdatedResponseDto;
        }

        public async Task<CustomerResponseDto> AddWallet(int clientId, string coin)
        {
            var customerFound = await((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(clientId);
            if (customerFound == null) throw new Exception("Customer does not exist");

            if (customerFound.Wallets.FindAll(x => x.Coin.Name == coin).Count > 0) throw new Exception("Wallet already exists");

            var coinFound = await ((UnitOfWork)_unitOfWork).CoinRepository.GetByName(coin);
            if (coinFound == null) throw new Exception("Coin does not exist");

            customerFound.Wallets.Add(new Wallet { Balance = 0m, BalanceHistory = new List<BalanceHistory>(), Coin = coinFound });

            _unitOfWork.Save();

            var customerFoundResponseEntity = _mapper.Map<CustomerResponseEntity>(customerFound);
            var customerFoundResponseDto = _mapper.Map<CustomerResponseDto>(customerFoundResponseEntity);

            return customerFoundResponseDto;
        }

        public async Task<Coin> AddCoin(string coin)
        {
            var newCoin = new Coin { Name = coin, BuyValue = 0, SellValue = 0 };
            var returnedCoin = await ((UnitOfWork)_unitOfWork).CoinRepository.Insert(newCoin);
            if (returnedCoin == null) throw new Exception("Failed to insert coin");

            _unitOfWork.Save();

            return returnedCoin;
        }

        public void RestoreDB()
        {
            _unitOfWork.EnsureDeleted();
            _unitOfWork.EnsureCreated();
            
            _unitOfWork.Save();
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

        public Dictionary<string, string> GetTokenClaims(string jwtToken)
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
            tokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);


            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                return jwtSecurityToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            }

            throw new Exception("Failed to get token info");
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