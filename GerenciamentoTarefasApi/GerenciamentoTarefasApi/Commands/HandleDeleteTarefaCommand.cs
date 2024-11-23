using MediatR;

namespace GerenciamentoTarefasApi.Commands
{
    public class HandleDeleteTarefaCommand : IRequest<bool>
    {
       public int TarefaId { get;}

        public HandleDeleteTarefaCommand(int tarefaId)
        {
            TarefaId = tarefaId;
        }
    }
}
