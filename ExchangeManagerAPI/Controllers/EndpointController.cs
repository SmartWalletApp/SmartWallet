using ExchangeManager.DomainModel.RepositoryContracts;
using ExchangeManager.Infrastructure.DataModels;
using ExchangeManager.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace ExchangeManagerAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EndpointController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EndpointController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetCustomers")]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await ((UnitOfWork)_unitOfWork).CustomerRepository.GetAll();
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<Customer> GetCustomer(int id)
        {
            return await ((UnitOfWork)_unitOfWork).CustomerRepository.GetByID(id);
        }

        [HttpPost(Name = "InsertCustomer")]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            return Ok(await ((UnitOfWork)_unitOfWork).CustomerRepository.Insert(customer));
        }

        [HttpDelete("{id}", Name = "DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            return Ok(await ((UnitOfWork)_unitOfWork).CustomerRepository.Delete(id));
        }

        [HttpPut(Name = "UpdateStudent")]
        public async Task<ActionResult<Customer>> UpdateStudent([FromBody] Customer newCustomer)
        {
            return Ok(await ((UnitOfWork)_unitOfWork).CustomerRepository.Update(newCustomer));
        }

#if DEBUG
        [HttpDelete(Name = "RestoreDB")]
        public IActionResult RestoreDB()
        {
            _unitOfWork.EnsureDeleted();
            _unitOfWork.EnsureCreated();
            //_unitOfWork.SetDefaultCoin();
            return Ok($"Restored successfully");
        }
        # endif

    }
}
