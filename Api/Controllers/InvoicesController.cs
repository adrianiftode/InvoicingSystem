using Api.Models;
using Core;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class InvoicesController : ApiController
    {
        private readonly IInvoicesService _invoicesService;

        public InvoicesController(IInvoicesService invoicesService)
        {
            _invoicesService = invoicesService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceModel>> Get(int id)
        {
            var invoice = await _invoicesService.Get(id);

            return OkOrNotFound(invoice?.Map());
        }

        [HttpGet("{id}/notes")]
        public async Task<ActionResult<IEnumerable<NoteModel>>> GetNotes(int id)
        {
            var notes = await _invoicesService.GetNotesBy(id);

            return OkOrNotFound(notes?.Map());

        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Invoice))]
        public async Task<ActionResult<InvoiceModel>> Create([FromBody]CreateInvoiceRequestModel request)
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

            return CreatedAtAction(nameof(Get), new { id = invoice.InvoiceId }, invoice.Map());
        }

        [HttpPut]
        [ProducesResponseType(201, Type = typeof(InvoiceModel))]
        public async Task<ActionResult<InvoiceModel>> Update([FromBody]UpdateInvoiceRequestModel request)
        {
            var invoice = await _invoicesService.Update(new UpdateInvoiceRequest
            {
                InvoiceId = request.InvoiceId,
                Amount = request.Amount,
                Identifier = request.Identifier,
                User = User
            });

            if (invoice == null)
            {
                return BadRequest(new { error = "Invoice could not be updated." });
            }

            return invoice.Map();
        }
    }
}
