using Api.Models;
using Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Api.Controllers
{
    public class InvoicesController : ApiController
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceModel>> Get(int id)
        {
            var invoice = await _mediator.Send(new InvoiceByIdQuery { Id = id });

            return OkOrNotFound(invoice?.Map());
        }

        [HttpGet("{id}/notes")]
        public async Task<ActionResult<IEnumerable<NoteModel>>> GetNotes(int id)
        {
            var notes = await _mediator.Send(new InvoiceNotesQuery { InvoiceId = id });

            return OkOrNotFound(notes?.Map());

        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(InvoiceModel))]
        public async Task<ActionResult<InvoiceModel>> Create([FromBody]CreateInvoiceRequestModel request)
        {
            var result = await _mediator.Send(new CreateInvoiceRequest
            {
                Amount = request.Amount,
                Identifier = request.Identifier
            });

            return CreatedResult(result, InvoiceMapper.Map, nameof(Get), new
            {
                id = result.Item?.InvoiceId
            });
        }

        [HttpPut]
        [ProducesResponseType(201, Type = typeof(InvoiceModel))]
        public async Task<ActionResult<InvoiceModel>> Update([FromBody]UpdateInvoiceRequestModel request)
        {
            var result = await _mediator.Send(new UpdateInvoiceRequest
            {
                InvoiceId = request.InvoiceId,
                Amount = request.Amount,
                Identifier = request.Identifier
            });

            return Result(result, InvoiceMapper.Map);
        }
    }
}
