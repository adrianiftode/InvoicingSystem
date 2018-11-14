﻿using System.Threading.Tasks;

namespace Core.Services
{
    public interface INotesService
    {
        Task<Note> Get(int id);
        Task<Result<Note>> Update(UpdateNoteRequest request);
        Task<Result<Note>> Create(CreateNoteRequest request);
    }
}
