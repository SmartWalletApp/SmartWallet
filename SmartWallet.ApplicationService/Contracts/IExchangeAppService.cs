using SmartWallet.ApplicationService.Dto.Request;
using SmartWallet.ApplicationService.Dto.Response;
using SmartWallet.Infrastructure.DataModels;

namespace SmartWallet.ApplicationService.Contracts
{
    public interface ISmartWalletAppService
    {
        public Task<IEnumerable<CustomerResponseDto>> GetCustomers();

        public Task<CustomerResponseDto> GetCustomerById(int id);

        public Task<CustomerResponseDto> InsertCustomer(CustomerRequestDto customer);

        public Task<CustomerResponseDto> DeleteCustomer(int id);

        public Task<CustomerResponseDto> UpdateCustomer(CustomerRequestDto newCustomer);

        public void RestoreDB();

        public Task<CustomerResponseDto> VerifyCustomerLogin(string givenEmail, string givenPassword);

        public Dictionary<string, string> GetTokenClaims(string jwtToken);

        string CreateToken(CustomerResponseDto validatedCustomer);

        public Task<CustomerResponseDto> AddWallet(int clientId, string coin);

        public Task<Coin> AddCoin(string coin);

        public Task<IEnumerable<Coin>> GetCoins();

        public Task<CustomerResponseDto> RemoveWallet(int clientId, string coin);
    }
}
