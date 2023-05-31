using CleanArchitecture.Aggregation.Application.Interfaces;
using System.Security.Claims;

namespace CleanArchitecture.Aggregation.Spa.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
        }

        public string UserId { get; }
    }
}
