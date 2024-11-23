using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.Commands;
using MediatR;

namespace GerenciamentoTarefasApi.Handlers
{
    public class CreateProjectCommandHandler : IRequestHandler<HandleCreateProjectCommand, Projeto>
    {
        private readonly GerenciamentoTarefasContext _context;

        public CreateProjectCommandHandler(GerenciamentoTarefasContext context)
        {
            _context = context;
        }

        public async Task<Projeto> Handle(HandleCreateProjectCommand request, CancellationToken cancellationToken)
        {
            var projeto = new Projeto
            {
                Nome = request.ProjetoDto.Nome,
                Descricao = request.ProjetoDto.Descricao,
                DataInicio = request.ProjetoDto.DataInicio,
                DataFim = request.ProjetoDto.DataFim,
                UserId = request.ProjetoDto.UserId,
            };

            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync(cancellationToken);

            return projeto;
        }
    }
}
