using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Core.Handlers
{
    public class CreateNoteHandler : IRequestHandler<CreateNoteRequest, Result<Note>>
    {
        public Task<Result<Note>> Handle(CreateNoteRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}