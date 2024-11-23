using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.Commands;
using GerenciamentoTarefasApi.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GerenciamentoTarefas.Test
{
    [TestFixture]
    public class UpdateTarefaCommandHandlerTests : IDisposable
    {
        private GerenciamentoTarefasContext _context;
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<GerenciamentoTarefasContext>()
                          .UseInMemoryDatabase(databaseName: "TestDb")
                          .Options;
            _context = new GerenciamentoTarefasContext(options);

            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(HandleCreateProjectCommand).Assembly));
            services.AddDbContext<GerenciamentoTarefasContext>(opt => opt.UseInMemoryDatabase("TestDb"));
            var serviceProvider = services.BuildServiceProvider();
            _mediator = serviceProvider.GetRequiredService<IMediator>();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        [Test]
        public async Task HandleUpdateTarefaCommand_WhenPriorityHasNotChange_ShouldNotThrowException()
        {
            var usuario = CriarUsuario();
            var projetoDto = new ProjetoDto
            {
                Nome = "Novo Projeto",
                Descricao = "Descrição do novo projeto",
                DataInicio = DateTime.UtcNow,
                DataFim = DateTime.UtcNow,
                UserId = usuario.Id
            };
            var projeto = CriarProjeto(projetoDto);
            var tarefaDto = new TarefaDto()
            {
                DataVencimento = DateTime.UtcNow,
                Descricao = "asdfasdf",
                Prioridade = PrioridadeTarefa.ALTA,
                ProjetoId = projetoDto.Id,
                Status = StatusTarefa.PENDENTE,
                Titulo = "Teste",
                UserId = usuario.Id
            };
            var tarefa = CriarTarefa(tarefaDto);
            tarefaDto.Id = tarefa.Id;
            var comando = new HandleUpdateTarefaCommand(tarefaDto);
            Assert.DoesNotThrow(() => _mediator.Send(comando));
            var tarefaAtualizada = await  _context.Tarefas.FindAsync(tarefa.Id);
            var historico = await _context.HistoricoAlteracoes.Where(h => h.TarefaId == tarefaAtualizada.Id).FirstOrDefaultAsync();
            Assert.IsNotNull(historico);
            Assert.AreEqual(tarefa.Prioridade, tarefaAtualizada.Prioridade);
        }

        [Test]
        public async Task HandleUpdateTarefaCommand_WhenACommentIsAdded_ShouldRegisterInChangeHistory()
        {
            var usuario = CriarUsuario();
            var projetoDto = new ProjetoDto
            {
                Nome = "Novo Projeto",
                Descricao = "Descrição do novo projeto",
                DataInicio = DateTime.UtcNow,
                DataFim = DateTime.UtcNow,
                UserId = usuario.Id
            };
            var projeto = CriarProjeto(projetoDto);
            var tarefaDto = new TarefaDto()
            {
                DataVencimento = DateTime.UtcNow,
                Descricao = "asdfasdf",
                Prioridade = PrioridadeTarefa.ALTA,
                ProjetoId = projetoDto.Id,
                Status = StatusTarefa.PENDENTE,
                Titulo = "Teste",
                UserId = usuario.Id
            };
            var tarefa = CriarTarefa(tarefaDto);
            tarefaDto.Id = tarefa.Id;
            var comando = new HandleUpdateTarefaCommand(tarefaDto);
            _mediator.Send(comando);
            var tarefaAtualizada = await _context.Tarefas.FindAsync(tarefa.Id);
            var historicos = await _context.HistoricoAlteracoes.Where(h => h.TarefaId == tarefa.Id).ToListAsync();
            Assert.IsTrue(historicos.Any(x => x.Alteracoes.Contains("comentário adicionado")));
        }

        [Test]
        public async Task HandleUpdateTarefaCommand_WhenPriorityHasChange_ShouldThrowException()
        {
            var usuario = CriarUsuario();
            var projetoDto = new ProjetoDto
            {
                Nome = "Novo Projeto",
                Descricao = "Descrição do novo projeto",
                DataInicio = DateTime.UtcNow,
                DataFim = DateTime.UtcNow,
                UserId = usuario.Id
            };
            var projeto = CriarProjeto(projetoDto);
            var tarefaDto = new TarefaDto()
            {
                DataVencimento = DateTime.UtcNow,
                Descricao = "asdfasdf",
                Prioridade = PrioridadeTarefa.ALTA,
                ProjetoId = projetoDto.Id,
                Status = StatusTarefa.PENDENTE,
                Titulo = "Teste",
                UserId = usuario.Id
            };
            var tarefa = CriarTarefa(tarefaDto);
            tarefaDto.Id = tarefa.Id;
            tarefaDto.Prioridade = PrioridadeTarefa.BAIXA;
            var comando = new HandleUpdateTarefaCommand(tarefaDto);
            var ex = Assert.Throws<Exception>(() => _mediator.Send(comando));
            Assert.That(ex.Message, Is.EqualTo("Não é permitido alterar a prioridade de uma tarefa"));

        }

        [TearDown]
        public void TearDown()
        {
            _context?.Database.EnsureDeleted();
        }

        private Usuario CriarUsuario()
        {
            Usuario usuario = new Usuario();
            usuario.UserName = "Admin";
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario;
        }

        private Projeto CriarProjeto(ProjetoDto dto)
        {
            var projeto = new Projeto
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                UserId = dto.UserId,
            };
            _context.Projetos.Add(projeto);
            _context.SaveChanges();
            return projeto;
        }

        private Tarefa CriarTarefa(TarefaDto dto)
        {
            var tarefa = new Tarefa();
            tarefa.Status = dto.Status;
            tarefa.Descricao = dto.Descricao;
            tarefa.ProjetoId = dto.ProjetoId;
            tarefa.Prioridade = dto.Prioridade;
            tarefa.DataVencimento = dto.DataVencimento;
            tarefa.Titulo = dto.Titulo;
            tarefa.UserId = dto.UserId;
            tarefa.Comentarios = dto.Comentarios != null?
                dto.Comentarios.Select(x => new Comentario()
                {
                    Texto = x.Texto,
                    TarefaId = tarefa.Id,
                }).ToList(): Enumerable.Empty<Comentario>().ToList();

            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return tarefa;
        }
    }
}
