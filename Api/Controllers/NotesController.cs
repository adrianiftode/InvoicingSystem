using Api.Models;
using Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Api.Controllers
{
    public class NotesController : ApiController
    {
        private readonly IMediator _mediator;

        public NotesController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteModel>> Get(int id)
            => OkOrNotFound((await _mediator.Send(new NoteByIdQuery { Id = id })).Map());

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NoteModel))]
        public async Task<ActionResult<NoteModel>> Create([FromBody] CreateNoteRequestModel requestModel)
        {
            var result = await _mediator.Send(new CreateNoteRequest
            {
                InvoiceId = requestModel.InvoiceId,
                Text = requestModel.Text
            });

            return CreatedResult(result, NoteMapper.Map, nameof(Get), new
            {
                id = result.Item?.NoteId
            });
        }


        [HttpPut]
        [ProducesResponseType(201, Type = typeof(NoteModel))]
        public async Task<ActionResult<NoteModel>> Update([FromBody] UpdateNoteRequestModel requestModel)
            => Result(await _mediator.Send(new UpdateNoteRequest
            {
                NoteId = requestModel.NoteId,
                Text = requestModel.Text
            }), NoteMapper.Map);
    }
}
