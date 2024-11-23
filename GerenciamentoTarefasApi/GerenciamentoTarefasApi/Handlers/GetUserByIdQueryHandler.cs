using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.DTOs;
using GerenciamentoTarefasApi.Queries;
using MediatR;

namespace GerenciamentoTarefasApi.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Usuario>
    {
        private readonly GerenciamentoTarefasContext _context;

        public GetUserByIdQueryHandler(GerenciamentoTarefasContext context)
        {
            _context = context;
        }

        public async Task<Usuario> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Usuarios.FindAsync(request.UserId);
        }
    }
}
