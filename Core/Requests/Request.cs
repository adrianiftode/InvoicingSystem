using System.Security.Claims;

namespace Core
{
    public class Request
    {
        public ClaimsPrincipal User { get; set; }
    }
}
