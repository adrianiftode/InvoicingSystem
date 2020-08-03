using Newtonsoft.Json;
using System.Security.Claims;

namespace Core
{
    public class Request
    {
        [JsonIgnore]
        public ClaimsPrincipal User { get; set; }
    }
}
