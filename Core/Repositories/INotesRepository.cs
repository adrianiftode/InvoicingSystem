using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface INotesRepository
    {
        Task<Note> Get(int id);
        Task Update();
        Task Create(Note note);
    }
}
