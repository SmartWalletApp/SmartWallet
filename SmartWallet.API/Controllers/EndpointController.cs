using SmartWallet.ApplicationService.Contracts;
using SmartWallet.Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
            if (ValidateToken())
            {
                return Ok(await _appService.GetCustomers());
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            if (ValidateToken())
            {
                return Ok(await _appService.GetCustomer(id));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost(Name = "InsertCustomer")]
        public async Task<ActionResult<Customer>> InsertCustomer(Customer customer)
        {
            if (ValidateToken())
            {
                return Ok(await _appService.InsertCustomer(customer));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}", Name = "DeleteCustomer")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            if (ValidateToken())
            {
                return Ok(await _appService.DeleteCustomer(id));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPut(Name = "UpdateStudent")]
        public async Task<ActionResult<Customer>> UpdateCustomer([FromBody] Customer newCustomer)
        {
            if (ValidateToken())
            {
                return Ok(await _appService.UpdateCustomer(newCustomer));
            }
            else
            {
                return Unauthorized();
            }
        }

        //#if DEBUG
        [HttpDelete(Name = "RestoreDB")]
        public async Task<ActionResult> RestoreDB()
        {
            if (ValidateToken())
            {
                await _appService.RestoreDB();
                return Ok("DB Restored");
            }
            else
            {
                return Unauthorized();
            }
        }
        //# endif

        // Log the user if credentials are ok
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

        [HttpGet("VerifyLogin")]
        public bool ValidateToken()
        {
            if (Request.Cookies.TryGetValue("jwt", out var jwtToken))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = _appService.GetTokenValidationParam();
                tokenHandler.ValidateToken(jwtToken, validationParameters, out _);
                return true;
            }
            return false;
        }
    }
}
