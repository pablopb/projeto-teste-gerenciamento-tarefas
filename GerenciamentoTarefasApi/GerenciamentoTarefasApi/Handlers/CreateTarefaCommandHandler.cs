using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefasApi.Handlers
{
    public class CreateTarefaCommandHandler : IRequestHandler<HandleCreateTarefaCommand, Tarefa>
    {
        private readonly GerenciamentoTarefasContext _context;
        private const int MAXIMO_TAREFAS_POR_PROJETO = 20;

        public CreateTarefaCommandHandler(GerenciamentoTarefasContext context)
        {
            _context = context;
        }

        public async Task<Tarefa> Handle(HandleCreateTarefaCommand request, CancellationToken cancellationToken)
        {
            var projeto = _context.Projetos
                .Include(x => x.Tarefas)
                .Where(x => x.Id == request.TarefaDto.ProjetoId)
                .FirstOrDefault();

            if (projeto.Tarefas.Count() == MAXIMO_TAREFAS_POR_PROJETO)
            {
                throw new Exception($"Não é permitido adicionar mais de {MAXIMO_TAREFAS_POR_PROJETO} tarefas por projeto");
            }


            var tarefa = new Tarefa();
            tarefa.Status = request.TarefaDto.Status;
            tarefa.Descricao = request.TarefaDto.Descricao;
            tarefa.ProjetoId = request.TarefaDto.ProjetoId;
            tarefa.Prioridade = request.TarefaDto.Prioridade;
            tarefa.DataVencimento = request.TarefaDto.DataVencimento;
            tarefa.Titulo = request.TarefaDto.Titulo;
            tarefa.UserId = request.TarefaDto.UserId;

            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            if (request.TarefaDto.Comentarios != null && request.TarefaDto.Comentarios.Any()){
                tarefa.Comentarios = request.TarefaDto.Comentarios.Select(x => new Comentario()
                {
                    Texto = x.Texto,
                    TarefaId = tarefa.Id,
                } ).ToList();
                _context.Comentarios.AddRange(tarefa.Comentarios);
                _context.SaveChanges();
            }
            return tarefa;
        }
    }
}
