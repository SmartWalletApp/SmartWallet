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
                    if (_appService.CheckJwtAuthentication(jwtToken))
                    {
                        return Ok(await _appService.GetCustomers());
                    }
                    return Unauthorized();

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
                    if (_appService.CheckJwtAuthentication(jwtToken))
                    {
                        return Ok(await _appService.GetCustomer(id));
                    }
                    return Unauthorized();

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
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    if (_appService.CheckJwtAuthentication(jwtToken))
                    {
                        return Ok(await _appService.DeleteCustomer(id));
                    }
                    return Unauthorized();

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
                    if (_appService.CheckJwtAuthentication(jwtToken))
                    {
                        return Ok(await _appService.UpdateCustomer(newCustomer));
                    }
                    return Unauthorized();

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
                    if (_appService.CheckJwtAuthentication(jwtToken))
                    {
                        await _appService.RestoreDB();
                        return Ok("DB Restored");
                    }
                    return Unauthorized();

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
                var tokenString = _appService.GetTokenString(validatedCustomer);

                // Add token to client cookie
                Response.Cookies.Append("jwt", tokenString, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                    Expires = DateTime.Now.AddDays(1)
                });
                return Ok(new { token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("ValidateWebLogin")]
        public IActionResult ValidateWebLogin()
        {
            try
            {
                if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
                {
                    return Ok(_appService.CheckJwtAuthentication(jwtToken));
                }
                return Unauthorized("No session found");
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
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
