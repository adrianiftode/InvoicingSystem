using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;


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
        [ProducesResponseType(201, Type = typeof(InvoiceModel))]
        public async Task<ActionResult<InvoiceModel>> Create([FromBody]CreateInvoiceRequestModel request)
        {
            var result = await _invoicesService.Create(new CreateInvoiceRequest
            {
                Amount = request.Amount,
                Identifier = request.Identifier,
                User = User
            });

            return CreatedResult(result, InvoiceMapper.Map, nameof(Get), new
            {
                id = result.Item.InvoiceId
            });
        }

        [HttpPut]
        [ProducesResponseType(201, Type = typeof(InvoiceModel))]
        public async Task<ActionResult<InvoiceModel>> Update([FromBody]UpdateInvoiceRequestModel request)
        {
            var result = await _invoicesService.Update(new UpdateInvoiceRequest
            {
                InvoiceId = request.InvoiceId,
                Amount = request.Amount,
                Identifier = request.Identifier,
                User = User
            });

            return Result(result, InvoiceMapper.Map);
        }
    }
}
