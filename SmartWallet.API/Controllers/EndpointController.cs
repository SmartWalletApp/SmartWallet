using SmartWallet.ApplicationService.Contracts;
using SmartWallet.Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using SmartWallet.ApplicationService.Dto.Request;

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
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenInfo(jwtToken);
                    if (claims != null)
                    {
                        int clientId = int.Parse(claims["id"]);
                        return Ok(await _appService.GetCustomerById(id));
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
        public async Task<ActionResult<Customer>> InsertCustomer(CustomerRequestDto customer)
        {
            return Ok(await _appService.InsertCustomer(customer));
        }

        [HttpDelete("{id}", Name = "DeleteCustomer")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenInfo(jwtToken);
                    if (claims != null)
                    {
                        int clientId = int.Parse(claims["id"]);
                        if (id == clientId || claims["group"] == "admin")

                            return Ok(await _appService.DeleteCustomer(id));
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
        public async Task<ActionResult<Customer>> UpdateCustomer([FromBody] CustomerRequestDto newCustomer)
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    var claims = _appService.GetTokenInfo(jwtToken);
                    if (claims != null)
                    {
                        var customerToChange = await _appService.GetCustomerByEmail(newCustomer.Email);
                        int clientId = int.Parse(claims["id"]);
                        if (customerToChange.Id == clientId || claims["group"] == "admin")
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
            var validatedCustomerDto = await _appService.VerifyCustomerLogin(givenEmail, givenPassword);

            if (validatedCustomerDto != null)
            {
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
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("ValidateWebLogin")]
        public IActionResult ValidateWebLoginAsync()
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
