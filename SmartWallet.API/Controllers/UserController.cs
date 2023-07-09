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
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken);
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
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken);
                    int clientId = int.Parse(claims["id"]);
                    return Ok(await _appService.GetCustomerById(clientId));
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateCustomer", Name = "CreateCustomer")]
        public async Task<ActionResult<Customer>> CreateCustomer(CustomerRequestDto customer)
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
        public async Task<ActionResult<Customer>> AddWallet(string coin)
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken);
                    int clientId = int.Parse(claims["id"]);
                    return Ok(await _appService.AddWallet(clientId, coin));
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("RemoveWallet/{coin}", Name = "RemoveWallet")]
        public async Task<ActionResult<Customer>> RemoveWallet(string coin)
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken);
                    int clientId = int.Parse(claims["id"]);
                    return Ok(await _appService.RemoveWallet(clientId, coin));
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

                Response.Cookies.Append("jwt", tokenString, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                    #if DEBUG
                    Expires = DateTime.Now.AddHours(6)
                    #else
                    Expires = DateTime.Now.AddMinutes(10)
                    #endif
                });
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
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenClaims(jwtToken);
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
                if (Request.Cookies.ContainsKey("jwt"))
                {
                    Response.Cookies.Delete("jwt");
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
