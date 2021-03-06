﻿using Core;
using Core.Repositories;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private readonly InvoicingContext _context;

        public NotesRepository(InvoicingContext context) => _context = context;

        public async Task<Note> Get(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            return note;
        }

        public async Task Create(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
        }

        public Task Update() => _context.SaveChangesAsync();
    }
}
