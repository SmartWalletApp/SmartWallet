using SmartWallet.ApplicationService.Contracts;
using SmartWallet.Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SmartWallet.API.JWT.Contracts;

namespace SmartWallet.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class EndpointController : ControllerBase
    {
        private readonly ISmartWalletAppService _appService;
        private readonly IJwtProperties _jwtProperties;

        public EndpointController(ISmartWalletAppService appService, IJwtProperties jwtProperties)
        {
            _appService = appService;
            _jwtProperties = jwtProperties;
        }

        [HttpGet(Name = "GetCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            if (ValidateToken().Value)
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
            if (ValidateToken().Value)
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
            if (ValidateToken().Value)
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
            if (ValidateToken().Value)
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
            if (ValidateToken().Value)
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
            if (ValidateToken().Value)
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
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, validatedCustomer.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtProperties.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds,
                    Issuer = _jwtProperties.Issuer
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                var tokenString = tokenHandler.WriteToken(token);

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

        // Verify the user is logged
        [HttpGet("VerifyLogin")]
        public ActionResult<bool> ValidateToken()
        {
            if (Request.Cookies.TryGetValue("jwt", out string jwtToken))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtProperties.Key);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = _jwtProperties.Issuer,
                };

                try
                {
                    var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
                    //return Ok("Token is valid");
                    return Ok();
                }
                catch (Exception ex)
                {
                    //return BadRequest("Token is invalid: " + ex.Message);
                    return ValidationProblem();
                }
            }
            else
            {
                //return BadRequest("No JWT cookie found");
                return Unauthorized();
            }
        }



    }
}
