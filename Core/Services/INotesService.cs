using System.Threading.Tasks;

namespace Core.Services
{
    public interface INotesService
    {
        Task<Note> Get(int id);
        Task<Note> Update(UpdateNoteRequest request);
        Task<Note> Create(CreateNoteRequest request);
    }
}
