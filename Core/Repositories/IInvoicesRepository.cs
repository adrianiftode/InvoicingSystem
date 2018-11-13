using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IInvoicesRepository
    {
        Task<Invoice> Get(int id);
        Task<IReadOnlyCollection<Note>> GetNotesBy(int invoiceId);
    }
}
