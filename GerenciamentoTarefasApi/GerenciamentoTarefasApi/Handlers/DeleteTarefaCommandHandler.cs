using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.Commands;
using MediatR;

namespace GerenciamentoTarefasApi.Handlers
{
    public class DeleteTarefaCommandHandler : IRequestHandler<HandleDeleteTarefaCommand, bool>
    {
        private readonly GerenciamentoTarefasContext _context;

        public DeleteTarefaCommandHandler(GerenciamentoTarefasContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(HandleDeleteTarefaCommand request, CancellationToken cancellationToken)
        {
            var tarefa = await _context.Tarefas.FindAsync(request.TarefaId);
            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);
                _context.SaveChanges();
            }
            return false;
        }
    }
}
