using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoicesService _invoicesService;

        public InvoicesController(IInvoicesService invoicesService)
        {
            _invoicesService = invoicesService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> Get(int id)
        {
            var invoice = await _invoicesService.Get(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        [HttpGet("{id}/notes")]
        public async Task<ActionResult<IReadOnlyCollection<Note>>> GetNotes(int id)
        {
            var notes = await _invoicesService.GetNotesBy(id);

            if (notes == null)
            {
                return NotFound();
            }

            return Ok(notes);
        }
    }
}
