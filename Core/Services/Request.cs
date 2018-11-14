using System.Security.Claims;

namespace Core.Services
{
    public class Request
    {
        public ClaimsPrincipal User { get; set; }
    }
}