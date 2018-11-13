using Api.Models;
using Core;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;

        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> Get(int id)
        {
            var note = await _notesService.Get(id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Note))]
        public async Task<ActionResult<Note>> Create([FromBody]CreateNoteRequestModel request)
        {
            var invoice = await _notesService.Create(new CreateNoteRequest
            {
                InvoiceId = request.InvoiceId,
                Text = request.Text,
                User = User
            });

            if (invoice == null)
            {
                return BadRequest(new { error = "Note could not be created." });
            }

            return CreatedAtAction(nameof(Get), new { id = invoice.NoteId }, invoice);
        }


        [HttpPut]
        [ProducesResponseType(201, Type = typeof(Note))]
        public async Task<ActionResult<Note>> Update([FromBody]UpdateNoteRequestModel request)
        {
            var note = await _notesService.Update(new UpdateNoteRequest
            {
                NoteId = request.NoteId,
                Text = request.Text,
                User = User
            });

            if (note == null)
            {
                return BadRequest(new { error = "Note could not be updated." });
            }

            return note;
        }
    }
}
