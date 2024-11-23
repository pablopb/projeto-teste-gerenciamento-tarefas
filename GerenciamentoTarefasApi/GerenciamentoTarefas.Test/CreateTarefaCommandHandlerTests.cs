using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Domain.Entity;
using GerenciamentoTarefasApi.Commands;
using GerenciamentoTarefasApi.DTOs;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GerenciamentoTarefas.Test
{
    [TestFixture]
    public class CreateTarefaCommandHandlerTests: IDisposable
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
        public async Task HandleCreateTarefaCommand_WhenTaskCountLessThanMaxAllowed_ShouldNotThrowException()
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
            for (var i = 0; i < 20; i++) {
                Assert.DoesNotThrow(() => _mediator.Send(comando));
            }
        }

        [Test]
        public async Task HandleCreateTarefaCommand_WhenTaskCountMoreThanMaxAllowed_ShouldThrowException()
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
            const int NUMERO_MAXIMO_TAREFAS = 20;
            for (var i = 0; i <= 20; i++)
            {
                if(i == 20)
                {
                    var ex = Assert.Throws<Exception>(() => _mediator.Send(comando));
                    Assert.That(ex.Message, Is.EqualTo($"Não é permitido adicionar mais de {NUMERO_MAXIMO_TAREFAS} tarefas por projeto"));
                }
                _mediator.Send(comando);
            }
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
            usuario.Perfil = Perfil.Gerente;
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
            tarefa.Comentarios = dto.Comentarios != null ?
                dto.Comentarios.Select(x => new Comentario()
                {
                    Texto = x.Texto,
                    TarefaId = tarefa.Id,
                }).ToList() : Enumerable.Empty<Comentario>().ToList();

            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return tarefa;
        }
    }
}
