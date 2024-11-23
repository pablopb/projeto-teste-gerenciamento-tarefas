using GerenciamentoTarefas.Data;
using GerenciamentoTarefasApi.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefasApi.Handlers
{
    public class DeleteProjetoCommandHandler : IRequestHandler<HandleDeleteProjetoCommand, bool>
    {
        private readonly GerenciamentoTarefasContext _context;

        public DeleteProjetoCommandHandler(GerenciamentoTarefasContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(HandleDeleteProjetoCommand request, CancellationToken cancellationToken)
        {
            var projeto = await _context.Projetos
                .Include(x => x.Tarefas)
                .Where(x => x.Id == request.ProjetoId)
                .FirstOrDefaultAsync();
            if (projeto != null) {
                if (projeto.Tarefas.Any())
                {
                    throw new Exception("O projeto possui tarefas e não pode ser excluído");
                }
                _context.Projetos.Remove(projeto);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
