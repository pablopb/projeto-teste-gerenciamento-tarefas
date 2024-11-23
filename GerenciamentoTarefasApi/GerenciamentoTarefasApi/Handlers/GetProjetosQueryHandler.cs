using GerenciamentoTarefas.Data;
using GerenciamentoTarefasApi.DTOs;
using GerenciamentoTarefasApi.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefasApi.Handlers
{
    public class GetProjetosQueryHandler : IRequestHandler<GetProjetosQuery, List<ProjetoDto>>
    {
        private readonly GerenciamentoTarefasContext _context;

        public GetProjetosQueryHandler(GerenciamentoTarefasContext context)
        {
            _context = context;
        }

        public async Task<List<ProjetoDto>> Handle(GetProjetosQuery request, CancellationToken cancellationToken)
        {
            var projetos = await _context.Projetos
                .Where(x => x.UserId == request.UserId)
                .Select(p => new ProjetoDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    DataFim = p.DataFim,
                    DataInicio = p.DataInicio
                })
                .ToListAsync(cancellationToken);

            return projetos;
        }
    }
}
