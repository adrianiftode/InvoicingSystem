using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    public class MeController : ControllerBase
    {
        [Route("me")]
        [HttpGet]
        public object Me() => User.Claims.Select(c => new
        {
            c.Type,
            c.Value
        }).ToList();
    }
}
