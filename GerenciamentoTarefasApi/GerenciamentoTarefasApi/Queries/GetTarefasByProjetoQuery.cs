using GerenciamentoTarefasApi.DTOs;
using MediatR;

namespace GerenciamentoTarefasApi.Queries
{
    public class GetTarefasByProjetoQuery : IRequest<List<TarefaDto>>
    {
        public int ProjetoId { get; set; }

        public GetTarefasByProjetoQuery(int projetoId)
        {
            ProjetoId = projetoId;
        }
    }
}
