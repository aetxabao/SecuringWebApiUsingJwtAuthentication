using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecuringWebApiUsingJwtAuthentication.Interfaces;
using SecuringWebApiUsingJwtAuthentication.Requests;
using System.Threading.Tasks;

namespace SecuringWebApiUsingJwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        /// <summary>
        /// Autentica a un cliente.
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     POST /login
        ///     {
        ///        "Username": "aetxabao",
        ///        "Password": "P4t4t4s!"
        ///     }
        ///
        /// </remarks>
        /// <param name="loginRequest"></param>
        /// <returns>Un objeto LoginResponse o string</returns>
        /// <response code="201">Un objeto con los datos de autenticación y el token</response>
        /// <response code="400">Un texto con el tipo de error</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Faltan datos");
            }

            var loginResponse = await customerService.Login(loginRequest);

            if (loginResponse == null)
            {
                return BadRequest("Credenciales erroneas");
            }

            return Ok(loginResponse);
        }
    }
}