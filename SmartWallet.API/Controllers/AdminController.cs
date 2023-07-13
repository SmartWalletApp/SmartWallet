using SmartWallet.ApplicationService.Contracts;
using SmartWallet.Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using SmartWallet.ApplicationService.Dto.Request;

namespace SmartWallet.API.Controllers
{

    [ApiController]
    [Route("[Controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ISmartWalletAppService _appService;

        public AdminController(ISmartWalletAppService appService)
        {
            _appService = appService;
        }

        [HttpPost("AddCoin/{coin}", Name = "AddCoin")]
        public async Task<ActionResult> AddCoin(string coin)
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    if (claims["group"] == "admin")
                    {
                        return Ok(await _appService.AddCoin(coin));
                    }
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCustomer/{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    if (claims["group"] == "admin")
                    {
                        return Ok(await _appService.GetCustomerById(id));
                    }
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCustomer/{id}", Name = "DeleteCustomer")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    if (claims["group"] == "admin")
                    {
                        return Ok(await _appService.DeleteCustomer(id));
                    }
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateCustomer/{id}", Name = "UpdateCustomer")]
        public async Task<ActionResult<Customer>> UpdateCustomer(int id, [FromBody] CustomerRequestDto newCustomer)
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    if (claims["group"] == "admin")
                    {
                        var customerToChange = await _appService.GetCustomerById(id);
                        return Ok(await _appService.UpdateCustomer(newCustomer));
                    }
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("RestoreDB", Name = "RestoreDB")]
        public async Task<ActionResult> RestoreDB()
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    if (claims["group"] == "admin")
                    {
                        await _appService.RestoreDB();
                        return Ok("DB Restored");
                    }
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
