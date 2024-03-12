using CleanArchitecture.Aggregation.Application.DTOs.Account;
using CleanArchitecture.Aggregation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Aggregation.WebApp.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    public class UserController : BaseApiController
    {
        private readonly IAccountService _accountService;
        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAddress()));
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
