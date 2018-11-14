using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IInvoicesService
    {
        Task<Invoice> Get(int id);
        Task<IReadOnlyCollection<Note>> GetNotesBy(int invoiceId);
        Task<Result<Invoice>> Update(UpdateInvoiceRequest request);
        Task<Result<Invoice>> Create(CreateInvoiceRequest request);
    }
}