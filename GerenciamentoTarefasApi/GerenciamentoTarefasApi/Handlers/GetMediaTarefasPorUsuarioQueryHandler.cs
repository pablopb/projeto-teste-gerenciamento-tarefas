using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefasApi.Handlers
{
    public class GetMediaTarefasPorUsuarioQueryHandler : IRequestHandler<GetMediaTarefasPorUsuarioQuery, decimal>
    {
        private readonly GerenciamentoTarefasContext _context;

        public GetMediaTarefasPorUsuarioQueryHandler(GerenciamentoTarefasContext context)
        {
            _context = context;
        }

        public async Task<decimal> Handle(GetMediaTarefasPorUsuarioQuery request, CancellationToken cancellationToken)
        {
            var trintaDiasAtras = DateTime.Now.AddDays(-30);

            var mediaTarefasConcluidasPorUsuario = _context.Tarefas
                .Where(t => t.Status == StatusTarefa.CONCLUIDA && t.DataVencimento >= trintaDiasAtras)
                .GroupBy(t => t.UserId)
                .Select(g => new
                {
                    UsuarioId = g.Key,
                    MediaConcluidas = g.Count()
                })
                .Average(x => x.MediaConcluidas);
            return (decimal)mediaTarefasConcluidasPorUsuario;
        }
    }
}
