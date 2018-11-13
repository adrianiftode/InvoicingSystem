using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IInvoicesRepository
    {
        Task<Invoice> Get(int id);
        Task<Invoice> GetByIdentifier(string identifier);
        Task<IReadOnlyCollection<Note>> GetNotesBy(int invoiceId);
        Task Create(Invoice invoice);
        Task Update();
    }
}
