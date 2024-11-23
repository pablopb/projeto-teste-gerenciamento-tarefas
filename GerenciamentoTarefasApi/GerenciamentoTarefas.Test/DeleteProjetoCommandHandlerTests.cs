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
    public class DeleteProjetoCommandHandlerTests : IDisposable
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
        public async Task HandleDeleteProjetoCommand_WhenProject_Has_No_Tasks_ShouldDeletProject()
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
            var comando = new HandleDeleteProjetoCommand(projeto.Id);
            Assert.DoesNotThrow(() => _mediator.Send(comando));
        }

        [Test]
        public async Task HandleDeleteProjetoCommand_WhenProject_Has_Tasks_ShouldThrowsException()
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
            var comando = new HandleCreateTarefaCommand(tarefaDto);
            _mediator.Send(comando);
            var comando2 = new HandleDeleteProjetoCommand(projeto.Id);
            var ex = Assert.Throws<Exception>(() => _mediator.Send(comando2));
            Assert.That(ex.Message, Is.EqualTo("O projeto possui tarefas e não pode ser excluído"));
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
    }
}
