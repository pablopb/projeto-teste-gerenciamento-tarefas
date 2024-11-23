using GerenciamentoTarefas.Domain.Entity;
using MediatR;

namespace GerenciamentoTarefasApi.Commands
{
    public class HandleDeleteProjetoCommand : IRequest<bool>
    {
        public int ProjetoId { get; }

        public HandleDeleteProjetoCommand(int projetoId)
        {
            ProjetoId = projetoId;
        }
    }
}
