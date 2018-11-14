﻿using Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<Result<Invoice>> Create(CreateInvoiceRequest request)
        {
            if (await _invoicesRepository.GetByIdentifier(request.Identifier) != null)
            {
                return Result<Invoice>.Error("The invoice cannot be created because another invoice with the same already exists.");
            }

            if (request.User.IsUser())
            {
                return Result<Invoice>.Forbidden;
            }

            var invoice = new Invoice
            {
                Amount = request.Amount,
                Identifier = request.Identifier,
                UpdatedBy = request.User.GetIdentity()
            };

            await _invoicesRepository.Create(invoice);

            return Result<Invoice>.Success(invoice);
        }

        public async Task<Result<Invoice>> Update(UpdateInvoiceRequest request)
        {
            var invoice = await _invoicesRepository.Get(request.InvoiceId);

            if (invoice == null)
            {
                return Result<Invoice>.NotPresent;
            }

            if (invoice.UpdatedBy != request.User.GetIdentity())
            {
                return Result<Invoice>.Forbidden;
            }

            var withNewIdentifier = await _invoicesRepository.GetByIdentifier(request.Identifier);

            if (withNewIdentifier != null && withNewIdentifier.InvoiceId != invoice.InvoiceId)
            {
                return Result<Invoice>.Error("The invoice cannot be updated because another invoice with the new identifier already exists.");
            }

            invoice.UpdatedBy = request.User.GetIdentity();
            invoice.Identifier = request.Identifier;
            invoice.Amount = request.Amount;

            await _invoicesRepository.Update();
            return Result<Invoice>.Success(invoice);
        }
    }
}