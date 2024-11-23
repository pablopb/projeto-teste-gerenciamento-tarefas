using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefasApi.Handlers
{
    public class UpdateTarefaCommandHandler : IRequestHandler<HandleUpdateTarefaCommand, Tarefa>
    {
        private readonly GerenciamentoTarefasContext _context;

        public UpdateTarefaCommandHandler(GerenciamentoTarefasContext context)
        {
            _context = context;
        }

        public async Task<Tarefa> Handle(HandleUpdateTarefaCommand request, CancellationToken cancellationToken)
        {
            var tarefa = await _context.Tarefas.FindAsync(request.TarefaDto.Id);
            if (tarefa.Prioridade != request.TarefaDto.Prioridade)
                throw new Exception("Não é permitido alterar a prioridade de uma tarefa");
            tarefa.Status = request.TarefaDto.Status;
            tarefa.Descricao = request.TarefaDto.Descricao;
            tarefa.ProjetoId = request.TarefaDto.ProjetoId;
            tarefa.DataVencimento = request.TarefaDto.DataVencimento;
            tarefa.Titulo = request.TarefaDto.Titulo;
            tarefa.UserId = request.TarefaDto.UserId;

            _context.Tarefas.Update(tarefa);
            _context.SaveChanges();
            var comentarios = _context.Comentarios.Where(x => x.TarefaId == tarefa.Id).ToList();
            foreach (var comentario in comentarios)
            {
                _context.Comentarios.Remove(comentario);
            }
            if (request.TarefaDto.Comentarios != null && request.TarefaDto.Comentarios.Any()) {
                var alteracoes = "";
                foreach (var comentario in request.TarefaDto.Comentarios) {
                    if(comentario.TarefaId == 0)
                    {
                        alteracoes += $"comentário adicionado => texto:{comentario.Texto}\n";
                    }
                }
              
                tarefa.Comentarios = request.TarefaDto.Comentarios.Select(x => new Comentario()
                {
                    Texto = x.Texto,
                    TarefaId = tarefa.Id,
                }).ToList();
                _context.Comentarios.AddRange(tarefa.Comentarios);
                _context.SaveChanges();
                if (!string.IsNullOrWhiteSpace(alteracoes))
                {
                    var historicoAlteracao = new HistoricoAlteracao()
                    {
                        DataAlteracao = DateTime.Now,
                        TarefaId = tarefa.Id,
                        UserId = request.TarefaDto.UserId,
                        Alteracoes = alteracoes
                    };
                    _context.HistoricoAlteracoes.Add(historicoAlteracao);
                    _context.SaveChanges();
                }
            }
            return tarefa;
        }
    }
}
