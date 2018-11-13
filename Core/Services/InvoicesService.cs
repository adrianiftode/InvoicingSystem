﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repositories;

namespace Core.Services
{
    public class InvoicesService : IInvoicesService
    {
        private readonly IInvoicesRepository _invoicesRepository;

        public InvoicesService(IInvoicesRepository invoicesRepository)
        {
            _invoicesRepository = invoicesRepository;
        }
        public async Task<Invoice> Get(int id)
        {
            var invoice = await _invoicesRepository.Get(id);
            return invoice;
        }

        public async Task<IReadOnlyCollection<Note>> GetNotesBy(int invoiceId)
        {
            var notes = await _invoicesRepository.GetNotesBy(invoiceId);
            return notes;
        }

        public async Task<AddNoteResponse> AddNote(AddNoteRequest request)
        {
            var invoice = await _invoicesRepository.Get(request.InvoiceId);

            var note = invoice.AddNote(request.Text, request.User.GetIdentity());

            //_invoicesRepository.Update(invoice);
            //var note = await _invoicesRepository.GetNotes()
            return new AddNoteResponse
            {
                //Item = invoice.Notes.LastOrDefault();
                Item = note
            };
        }
    }
}