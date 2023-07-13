using SmartWallet.ApplicationService.Contracts;
using SmartWallet.Infrastructure.DataModels;
using SmartWallet.DomainModel.RepositoryContracts;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SmartWallet.ApplicationService.Dto.Response;
using AutoMapper;
using SmartWallet.ApplicationService.Dto.Request;
using SmartWallet.DomainModel.Dto.Request;
using SmartWallet.DomainModel.Entities.Response;
using SmartWallet.Infrastructure.Persistence;
using SmartWallet.ApplicationService.JWT.Contracts;
using FluentValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartWallet.ApplicationService.Services
{
    public class SmartWalletAppService : ISmartWalletAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProperties _jwtProperties;
        private readonly IMapper _mapper;
        private readonly IValidator<CustomerRequestDto> _customerValidator;
        private readonly IValidator<CoinRequestDto> _coinValidator;
        private readonly IValidator<BalanceHistoricRequestDto> _BalanceHistoricValidator;

        public SmartWalletAppService(
            IUnitOfWork unitOfWork,
            IJwtProperties jwtProperties,
            IMapper mapper,
            IValidator<CustomerRequestDto> customerValidator,
            IValidator<CoinRequestDto> coinValidator,
            IValidator<BalanceHistoricRequestDto> BalanceHistoricValidator
            )
        {
            _unitOfWork = unitOfWork;
            _jwtProperties = jwtProperties;
            _mapper = mapper;
            _customerValidator = customerValidator;
            _coinValidator = coinValidator;
            _BalanceHistoricValidator = BalanceHistoricValidator;
        }

        public async Task<Dictionary<string, KeyValuePair<decimal, List<BalanceHistoricResponseDto>>>> GetBalanceHistorics(int customerId, string coinName, DateTime minDate, DateTime maxDate)
        {
            if (minDate == default || maxDate == default)
            {
                minDate = DateTime.Now.AddDays(-7);
                maxDate = DateTime.Now.AddDays(1);
            }

            var balanceHistorics = await ((UnitOfWork)_unitOfWork).WalletRepository.GetBalanceHistorics(customerId, coinName, minDate, maxDate) ?? throw new Exception("Customer or coin does not exist");

            var result = balanceHistorics.ToDictionary(
                bh => bh.Key,
                bh => new KeyValuePair<decimal, List<BalanceHistoricResponseDto>>(
                    bh.Value.Key,
                    bh.Value.Value.Select(_mapper.Map<BalanceHistoricResponseDto>).ToList()
                )
            );

            return result;
        }

        public async Task<CustomerResponseDto> RemoveHistoric(int customerId, int historicId, string coin)
        {
            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(customerId, true) ?? throw new Exception("Customer does not exist");

            var walletFound = customerFound.Wallets.Find(x => x.Coin.Name == coin) ?? throw new Exception("Wallet with that coin does not exist");

            var historicToDelete = walletFound.BalanceHistorics.FirstOrDefault(h => h.Id == historicId) ?? throw new Exception("BalanceHistoric with that id does not exist");

            walletFound.BalanceHistorics.Remove(historicToDelete);

            if (historicToDelete.IsIncome)
                walletFound.Balance -= historicToDelete.Variation;
            else
                walletFound.Balance += historicToDelete.Variation;

            _unitOfWork.Save();

            var customerFoundResponseEntity = _mapper.Map<CustomerResponseEntity>(customerFound);
            var customerFoundResponseDto = _mapper.Map<CustomerResponseDto>(customerFoundResponseEntity);

            return customerFoundResponseDto;
        }

        public async Task<CustomerResponseDto> AddHistoric(int customerId, BalanceHistoricRequestDto historic, string coin)
        {
            var validationResult = _BalanceHistoricValidator.Validate(historic);

            if (!validationResult.IsValid) throw new FormatException(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));

            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(customerId, true) ?? throw new Exception("Customer does not exist");

            var walletFound = customerFound.Wallets.Find(x => x.Coin.Name == coin) ?? throw new Exception("Wallet with that coin does not exist");

            var newHistoricRequestEntity = _mapper.Map<BalanceHistoricRequestEntity>(historic);
            var newHistoric = _mapper.Map<BalanceHistoric>(newHistoricRequestEntity);

            walletFound.BalanceHistorics.Add(newHistoric);

            if (newHistoric.IsIncome)
                walletFound.Balance += newHistoric.Variation;
            else
                walletFound.Balance -= newHistoric.Variation;

            _unitOfWork.Save();

            var customerFoundResponseEntity = _mapper.Map<CustomerResponseEntity>(customerFound);
            var customerFoundResponseDto = _mapper.Map<CustomerResponseDto>(customerFoundResponseEntity);

            return customerFoundResponseDto;
        }

        public async Task<CustomerResponseDto> RemoveWallet(int customerId, string coin)
        {
            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(customerId) ?? throw new Exception("Customer does not exist");
            var walletsToRemove = customerFound.Wallets.Find(x => x.Coin.Name == coin) ?? throw new Exception("No wallet with that coin found");
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
            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByEmail(givenEmail) ?? throw new Exception("Wrong user or password");
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

        public async Task<CustomerResponseDto> GetCustomerById(int id)
        {
            var customerFound = await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(id) ?? throw new Exception("Customer does not exist");
            var customerFoundResponseEntity = _mapper.Map<CustomerResponseEntity>(customerFound);
            var customerFoundResponseDto = _mapper.Map<CustomerResponseDto>(customerFoundResponseEntity);

            return customerFoundResponseDto;
        }

        public async Task<CustomerResponseDto> InsertCustomer(CustomerRequestDto newCustomer)
        {
            var validationResult = _customerValidator.Validate(newCustomer);
            
            if(!validationResult.IsValid) throw new FormatException(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));
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
                    BalanceHistorics = new List<BalanceHistoric>()
                };
                customer.Wallets.Add(wallet);
            }
            var customerInserted = await ((UnitOfWork)_unitOfWork).CustomerRepository.Insert(customer) ?? throw new Exception("Failed to insert customer");
            _unitOfWork.Save();
            var customerInsertedResponseEntity = _mapper.Map<CustomerResponseEntity>(customerInserted);
            var customerInsertedResponseDto = _mapper.Map<CustomerResponseDto>(customerInsertedResponseEntity);
            return customerInsertedResponseDto;
        }

        public async Task<CustomerResponseDto> DeleteCustomer(int id)
        {
            var customerDeleted = await ((UnitOfWork)_unitOfWork).CustomerRepository.Delete(id) ?? throw new Exception("Failed to delete customer");
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

            var customerUpdated = await ((UnitOfWork)_unitOfWork).CustomerRepository.UpdateAsync(customer) ?? throw new Exception("Failed to update customer");
            _unitOfWork.Save();

            var customerUpdatedResponseEntity = _mapper.Map<CustomerResponseEntity>(customerUpdated);
            var customerUpdatedResponseDto = _mapper.Map<CustomerResponseDto>(customerUpdatedResponseEntity);

            return customerUpdatedResponseDto;
        }

        public async Task<CustomerResponseDto> AddWallet(int customerId, string coin)
        {
            var customerFound = await((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(customerId) ?? throw new Exception("Customer does not exist");
            if (customerFound.Wallets.FindAll(x => x.Coin.Name == coin).Count > 0) throw new Exception("Wallet already exists");

            var coinFound = await ((UnitOfWork)_unitOfWork).CoinRepository.GetByName(coin) ?? throw new Exception("Coin does not exist");
            customerFound.Wallets.Add(new Wallet { Balance = 0m, BalanceHistorics = new List<BalanceHistoric>(), Coin = coinFound });

            _unitOfWork.Save();

            var customerFoundResponseEntity = _mapper.Map<CustomerResponseEntity>(customerFound);
            var customerFoundResponseDto = _mapper.Map<CustomerResponseDto>(customerFoundResponseEntity);

            return customerFoundResponseDto;
        }

        public async Task<Coin> AddCoin(string coinName)
        {
            var newCoin = new CoinRequestDto { Name = coinName, BuyValue = 0, SellValue = 0 };
            var validationResult = _coinValidator.Validate(newCoin);

            if (!validationResult.IsValid) throw new FormatException(string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage)));

            var newCoinResponseEntity = _mapper.Map<CoinRequestEntity>(newCoin);
            var coin = _mapper.Map<Coin>(newCoinResponseEntity);

            var returnedCoin = await ((UnitOfWork)_unitOfWork).CoinRepository.Insert(coin) ?? throw new Exception("Failed to insert coin");
            _unitOfWork.Save();

            return returnedCoin;
        }

        public Task RestoreDB()
        {
            _unitOfWork.EnsureDeleted();
            _unitOfWork.EnsureCreated();
            
            _unitOfWork.Save();
            return Task.CompletedTask;
        }

        private static string HashPassword(string unhashedPass)
        {
            // Currently bcrypt will use Cost 11 (2048 iteratios) by default, which is around 140ms in debug using a Ryzen 5 3600X and DDR4 3200MHz RAM.
            return BCrypt.Net.BCrypt.HashPassword(unhashedPass);
        }
        private static bool VerifyPassword(string unhashedText, string hashedText)
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
            tokenHandler.ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);


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