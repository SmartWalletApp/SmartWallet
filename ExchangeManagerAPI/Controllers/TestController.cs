using ExchangeManager.DomainModel.RepositoryContracts;
using ExchangeManager.Infrastructure.DataModels;
using ExchangeManager.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExchangeManagerAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetAllCoins")]
        public Task<IEnumerable<Customer>> GetAllCoins()
        {
            ((UnitOfWork)_unitOfWork).CustomerRepository.Insert(new Customer
            {
                Email= "eduarqué@gmail.com",
                Name = "Eduardo",
                Surname = "Arqué",
                Password = "1234",
            });
            return ((UnitOfWork)_unitOfWork).CustomerRepository.GetAll();
        }

        [HttpDelete]
        public IActionResult RestoreDB()
        {
            _unitOfWork.EnsureDeleted();
            _unitOfWork.EnsureCreated();
            _unitOfWork.SetCoins();
            return Ok($"Restored successfully");
        }
    }
}
