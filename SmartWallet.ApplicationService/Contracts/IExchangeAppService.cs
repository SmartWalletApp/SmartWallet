using SmartWallet.ApplicationService.Dto.Request;
using SmartWallet.ApplicationService.Dto.Response;

namespace SmartWallet.ApplicationService.Contracts
{
    public interface ISmartWalletAppService
    {
        public Task<IEnumerable<CustomerResponseDto>> GetCustomers();

        public Task<CustomerResponseDto> GetCustomerById(int id);
        public Task<CustomerResponseDto> GetCustomerByEmail(string email);

        public Task<CustomerResponseDto> InsertCustomer(CustomerRequestDto customer);

        public Task<CustomerResponseDto> DeleteCustomer(int id);

        public Task<CustomerResponseDto> UpdateCustomer(CustomerRequestDto newCustomer);

        public Task RestoreDB();

        public Task<CustomerResponseDto> VerifyCustomerLogin(string givenEmail, string givenPassword);

        Dictionary<string, string> GetTokenInfo(string? jwtToken);
        string CreateToken(CustomerResponseDto validatedCustomer);
    }
}
