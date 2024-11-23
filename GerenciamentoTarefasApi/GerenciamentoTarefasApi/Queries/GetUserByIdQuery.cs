using GerenciamentoTarefas.Domain.Entity;
using MediatR;

namespace GerenciamentoTarefasApi.Queries
{
    public class GetUserByIdQuery : IRequest<Usuario>
    {
        public int UserId { get; }

        public GetUserByIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}
