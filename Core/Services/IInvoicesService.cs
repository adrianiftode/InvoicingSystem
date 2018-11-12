using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IInvoicesService
    {
        Task<Invoice> Get(int id);
        Task<IReadOnlyCollection<Note>> GetNotes(int id);
    }
}