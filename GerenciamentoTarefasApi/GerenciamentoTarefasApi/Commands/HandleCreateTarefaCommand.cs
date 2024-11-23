using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.DTOs;
using MediatR;

namespace GerenciamentoTarefasApi.Commands
{
    public class HandleCreateTarefaCommand : IRequest<Tarefa>
    {
        public TarefaDto TarefaDto { get;}

        public HandleCreateTarefaCommand(TarefaDto tarefaDto)
        {
            TarefaDto = tarefaDto;
        }
    }
}
