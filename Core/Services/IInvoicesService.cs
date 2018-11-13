using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IInvoicesService
    {
        Task<Invoice> Get(int id);
        Task<IReadOnlyCollection<Note>> GetNotesBy(int invoiceId);
        Task<Invoice> Update(UpdateInvoiceRequest request);
        Task<Invoice> Create(CreateInvoiceRequest request);
    }
}