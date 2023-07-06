using SmartWallet.ApplicationService.Contracts;
using SmartWallet.Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace SmartWallet.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EndpointController : ControllerBase
    {
        private readonly ISmartWalletAppService _appService;

        public EndpointController(ISmartWalletAppService appService)
        {
            _appService = appService;
        }

        [HttpGet(Name = "GetCustomers")]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _appService.GetCustomers();
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<Customer> GetCustomer(int id)
        {
            return await _appService.GetCustomer(id);
        }

        [HttpPost(Name = "InsertCustomer")]
        public async Task<ActionResult<Customer>> InsertCustomer(Customer customer)
        {
            return Ok(await _appService.InsertCustomer(customer));
        }

        [HttpDelete("{id}", Name = "DeleteCustomer")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            return Ok(await _appService.DeleteCustomer(id));
        }

        [HttpPut(Name = "UpdateStudent")]
        public async Task<ActionResult<Customer>> UpdateCustomer([FromBody] Customer newCustomer)
        {
            return Ok(await _appService.UpdateCustomer(newCustomer));
        }

        #if DEBUG
        [HttpDelete(Name = "RestoreDB")]
        public async Task<ActionResult> RestoreDB()
        {
            await _appService.RestoreDB();
            return Ok("DB Restored");
        }
        # endif

    }
}
