using GerenciamentoTarefasApi.DTOs;
using MediatR;

namespace GerenciamentoTarefasApi.Queries
{
    public class GetProjetosQuery : IRequest<List<ProjetoDto>>
    {
        public int UserId { get; }

        public GetProjetosQuery(int userId) {
            UserId = userId;
        }
    }
}
