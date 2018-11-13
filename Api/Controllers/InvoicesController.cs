using Api.Models;
using Core;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Invoice))]
        [ProducesResponseType(400, Type = typeof(Invoice))]
        public async Task<ActionResult<Invoice>> Create([FromBody]CreateInvoiceRequestModel request)
        {
            var invoice = await _invoicesService.Create(new CreateInvoiceRequest
            {
                Amount = request.Amount,
                Identifier = request.Identifier,
                User = User
            });

            if (invoice == null)
            {
                return BadRequest(new { error = "Invoice could not be created." });
            }

            return CreatedAtAction(nameof(Get), new { id = invoice.InvoiceId }, invoice);
        }
    }
}
