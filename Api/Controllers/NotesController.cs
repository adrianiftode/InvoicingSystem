﻿using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Core;


namespace Api.Controllers
{
    public class NotesController : ApiController
    {
        private readonly INotesService _notesService;

        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteModel>> Get(int id)
        {
            var note = await _notesService.Get(id);

            return OkOrNotFound(note.Map());
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NoteModel))]
        public async Task<ActionResult<NoteModel>> Create([FromBody]CreateNoteRequestModel request)
        {
            
            var result = await _notesService.Create(new CreateNoteRequest
            {
                InvoiceId = request.InvoiceId,
                Text = request.Text,
                User = User
            });

            return CreatedResult(result, NoteMapper.Map, nameof(Get), new
            {
                id = result.Item?.NoteId
            });

        }


        [HttpPut]
        [ProducesResponseType(201, Type = typeof(NoteModel))]
        public async Task<ActionResult<NoteModel>> Update([FromBody]UpdateNoteRequestModel request)
        {
            var result = await _notesService.Update(new UpdateNoteRequest
            {
                NoteId = request.NoteId,
                Text = request.Text,
                User = User
            });

            return Result(result, NoteMapper.Map);
        }
    }
}
