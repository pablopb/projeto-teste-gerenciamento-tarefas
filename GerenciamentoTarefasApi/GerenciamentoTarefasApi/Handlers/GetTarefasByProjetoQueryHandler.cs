using GerenciamentoTarefas.Data;
using GerenciamentoTarefasApi.DTOs;
using GerenciamentoTarefasApi.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefasApi.Handlers
{
    public class GetTarefasByProjetoQueryHandler : IRequestHandler<GetTarefasByProjetoQuery, List<TarefaDto>>
    {
        private readonly GerenciamentoTarefasContext _context;

        public GetTarefasByProjetoQueryHandler(GerenciamentoTarefasContext context)
        {
            _context = context;
        }

        public async  Task<List<TarefaDto>> Handle(GetTarefasByProjetoQuery request, CancellationToken cancellationToken)
        {
            var tarefas = _context.Tarefas
                .AsNoTracking()
                .Where(x => x.ProjetoId == request.ProjetoId)
                .Include(x => x.HistoricoAlteracoes)
                .ThenInclude(y => y.Usuario);

            return tarefas.Select(t => new TarefaDto()
            {
                Id = t.Id,
                Prioridade = t.Prioridade,
                Status = t.Status,
                Descricao = t.Descricao,
                DataVencimento = t.DataVencimento,
                Titulo = t.Titulo,
                Comentarios = t.Comentarios.Select(x => new ComentarioDto()
                {
                    Id = x.Id,
                    Texto = x.Texto
                }).ToList(),
                HistoricoAlteracoes = t.HistoricoAlteracoes.Select(h =>
                    new HistoricoAlteracaoDto()
                    {
                        DataAlteracao = h.DataAlteracao,
                        TarefaId    = h.TarefaId,
                        UserId = h.UserId,
                        UserName = h.Usuario.UserName,
                        Alteracoes = h.Alteracoes,
                        Id = h.Id
                    }
                ).ToList()
            }).ToList();
        }
    }
}
