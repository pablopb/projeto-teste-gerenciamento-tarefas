using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.DTOs;
using MediatR;

namespace GerenciamentoTarefasApi.Commands
{
    public class HandleUpdateTarefaCommand : IRequest<Tarefa>
    {
        public TarefaDto TarefaDto { get; }

        public HandleUpdateTarefaCommand(TarefaDto tarefaDto)
        {
            TarefaDto = tarefaDto;
        }
    }
}
