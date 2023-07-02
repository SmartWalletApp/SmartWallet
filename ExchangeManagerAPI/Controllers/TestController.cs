using ExchangeManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExchangeManagerAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ExchangeManagerDbContext _dbContext;

        public TestController(ExchangeManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpDelete]
        public IActionResult RestoreDB()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
            return Ok($"Restored successfully");
        }
    }
}
