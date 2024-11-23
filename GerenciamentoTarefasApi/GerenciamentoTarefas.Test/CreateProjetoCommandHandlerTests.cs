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
    public class CreateProjetoCommandHandlerTests : IDisposable
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

        [Test]
        public async Task HandleCreateProjectCommand_ShouldCreateProjeto()
        {
            var usuario = CriarUsuarioTeste();
            var projetoDto = new ProjetoDto
            {
                Nome = "Novo Projeto",
                Descricao = "Descrição do novo projeto",
                DataInicio = DateTime.UtcNow,
                DataFim = DateTime.UtcNow,
                UserId = usuario.Id
            };

            var comando = new HandleCreateProjectCommand(projetoDto);

            var projetoCriado = await _mediator.Send(comando);

            var projetoNoDb = await _context.Projetos.FirstOrDefaultAsync(p => p.Id == projetoCriado.Id);
            Assert.NotNull(projetoNoDb);
            Assert.AreEqual(projetoDto.Nome, projetoNoDb.Nome);
            Assert.AreEqual(projetoDto.Descricao, projetoNoDb.Descricao);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Database.EnsureDeleted(); 
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        private Usuario CriarUsuarioTeste()
        {
            Usuario usuario = new Usuario();
            usuario.UserName = "Admin";
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario;
        }
    }
}
