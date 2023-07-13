using SmartWallet.ApplicationService.Dto.Request;
using SmartWallet.ApplicationService.Dto.Response;
using SmartWallet.DomainModel.Entities.Response;
using SmartWallet.Infrastructure.DataModels;

namespace SmartWallet.ApplicationService.Contracts
{
    public interface ISmartWalletAppService
    {

        public Task<CustomerResponseDto> GetCustomerById(int id);

        public Task<CustomerResponseDto> InsertCustomer(CustomerRequestDto customer);

        public Task<CustomerResponseDto> DeleteCustomer(int id);

        public Task<CustomerResponseDto> UpdateCustomer(CustomerRequestDto newCustomer);

        public Task RestoreDB();

        public Task<CustomerResponseDto> VerifyCustomerLogin(string givenEmail, string givenPassword);

        public Dictionary<string, string> GetTokenClaims(string jwtToken);

        string CreateToken(CustomerResponseDto validatedCustomer);

        public Task<CustomerResponseDto> AddWallet(int customerId, string coin);

        public Task<Coin> AddCoin(string coin);

        public Task<IEnumerable<Coin>> GetCoins();

        public Task<CustomerResponseDto> RemoveWallet(int customerId, string coin);

        public Task<CustomerResponseDto> AddHistoric(int customerId, BalanceHistoricRequestDto historic, string coin);

        public Task<Dictionary<string, BalanceHistoricCategoryEntity<decimal, List<BalanceHistoricResponseDto>>>> GetBalanceHistorics(int customerId, string coinName, DateTime minDate, DateTime maxDate);

        public Task<CustomerResponseDto> RemoveHistoric(int customerId, int historicId, string coin);
    }
}
