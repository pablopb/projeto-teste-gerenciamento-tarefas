using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.DTOs;
using MediatR;

namespace GerenciamentoTarefasApi.Commands
{
    public class HandleCreateProjectCommand : IRequest<Projeto>
    {
        public ProjetoDto ProjetoDto { get; }

        public HandleCreateProjectCommand(ProjetoDto projetoDto)
        {
            ProjetoDto = projetoDto;
        }
    }
}
