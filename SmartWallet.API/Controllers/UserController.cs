using SmartWallet.ApplicationService.Contracts;
using SmartWallet.Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using SmartWallet.ApplicationService.Dto.Request;

namespace SmartWallet.API.Controllers
{

    [ApiController]
    [Route("[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly ISmartWalletAppService _appService;

        public UserController(ISmartWalletAppService appService)
        {
            _appService = appService;
        }

        [HttpGet("GetCoins", Name = "GetCoins")]
        public async Task<IActionResult> GetCoins()
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    return Ok(await _appService.GetCoins());
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMyCustomer", Name = "GetMyCustomer")]
        public async Task<IActionResult> GetMyCustomer()
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    var customerId = int.Parse(claims["id"]);
                    return Ok(await _appService.GetCustomerById(customerId));
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetBalanceHistorics/{coin}", Name = "GetBalanceHistorics")]
        public async Task<IActionResult> GetBalanceHistorics(string coin, [FromQuery]DateTime since, [FromQuery]DateTime until)
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    var customerId = int.Parse(claims["id"]);
                    return Ok(await _appService.GetBalanceHistorics(customerId, coin, since, until));
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateCustomer", Name = "CreateCustomer")]
        public async Task<IActionResult> CreateCustomer(CustomerRequestDto customer)
        {
            try
            {
                return Ok(await _appService.InsertCustomer(customer));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddWallet/{coin}", Name = "AddWallet")]
        public async Task<IActionResult> AddWallet(string coin)
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    var customerId = int.Parse(claims["id"]);
                    return Ok(await _appService.AddWallet(customerId, coin));
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddHistoric/{coin}", Name = "AddHistoric")]
        public async Task<IActionResult> AddHistoric([FromBody] BalanceHistoricRequestDto historic, string coin)
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    var customerId = int.Parse(claims["id"]);
                    return Ok(await _appService.AddHistoric(customerId, historic, coin));
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("RemoveWallet/{coin}", Name = "RemoveWallet")]
        public async Task<IActionResult> RemoveWallet(string coin)
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    var customerId = int.Parse(claims["id"]);
                    return Ok(await _appService.RemoveWallet(customerId, coin));
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login", Name = "Login")]
        public async Task<IActionResult> Login(string givenEmail, string givenPassword)
        {
            try
            {
                var validatedCustomerDto = await _appService.VerifyCustomerLogin(givenEmail, givenPassword);

                var tokenString = _appService.CreateToken(validatedCustomerDto);

                Response.Headers.Add("Authorization", tokenString);

                #if DEBUG
                Response.Headers.Add("Token-Expires", DateTime.Now.AddHours(6).ToString());
                #else
                Response.Headers.Add("Token-Expires", DateTime.Now.AddMinutes(10).ToString());
                #endif

                return Ok(new { token = tokenString });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpGet("ValidateWebLogin")]
        public IActionResult ValidateWebLoginAsync()
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken!);
                    return Ok(claims);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        [HttpPost("Logout", Name = "Logout")]
        public IActionResult Logout()
        {
            try
            {
                if (Request.Headers.ContainsKey("Authorization"))
                {
                    Response.Headers.Remove("Authorization");
                    return Ok("Logout successful");
                }
                return BadRequest("No active session found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
