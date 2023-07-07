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
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenInfo(jwtToken);
                    if (claims != null)
                    {
                        if (claims["group"] == "admin")
                        {
                            return Ok(await _appService.GetCustomers());
                        }
                    }
                }
                return Unauthorized();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer()
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenInfo(jwtToken);
                    if (claims != null)
                    {
                        int clientId = Int32.Parse(claims["id"]);
                        return Ok(await _appService.GetCustomer(clientId));
                    }
                }
                return Unauthorized();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost(Name = "InsertCustomer")]
        public async Task<ActionResult<Customer>> InsertCustomer(Customer customer)
        {
            return Ok(await _appService.InsertCustomer(customer));
        }

        [HttpDelete("{id}", Name = "DeleteCustomer")]
        public async Task<ActionResult<Customer>> DeleteCustomer()
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenInfo(jwtToken);
                    if (claims != null)
                    {
                        int clientId = Int32.Parse(claims["id"]);
                        return Ok(await _appService.DeleteCustomer(clientId));
                    }
                }
                return Unauthorized();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPut(Name = "UpdateStudent")]
        public async Task<ActionResult<Customer>> UpdateCustomer([FromBody] Customer newCustomer)
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenInfo(jwtToken);
                    if (claims != null)
                    {
                        int clientId = Int32.Parse(claims["id"]);
                        if (newCustomer.Id == clientId)
                        {
                            return Ok(await _appService.UpdateCustomer(newCustomer));
                        }
                    }
                }
                return Unauthorized();
            }
            catch
            {
                return Unauthorized();
            }
        }

        // For debugging purposes.
        [HttpDelete(Name = "RestoreDB")]
        public async Task<ActionResult> RestoreDB()
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenInfo(jwtToken);
                    if (claims != null)
                    {
                        if (claims["group"] == "admin")
                        {
                            await _appService.RestoreDB();
                            return Ok("DB Restored");
                        }
                    }
                }
                return Unauthorized();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string givenEmail, string givenPassword)
        {
            var validatedCustomer = await _appService.VerifyCustomerLogin(givenEmail, givenPassword);
            if (validatedCustomer != null)
            {
                var tokenString = _appService.CreateToken(validatedCustomer);

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
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("ValidateWebLogin")]
        public async Task<IActionResult> ValidateWebLoginAsync()
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenInfo(jwtToken);
                    if (claims != null)
                    {
                        return Ok(claims);
                    }
                }
                return Unauthorized();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("jwt"))
            {
                Response.Cookies.Delete("jwt");
                return Ok("Logout successful");
            }
            return BadRequest("No active session found");
        }

    }
}
