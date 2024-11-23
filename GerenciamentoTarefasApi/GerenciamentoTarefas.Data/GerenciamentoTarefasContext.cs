using GerenciamentoTarefas.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefas.Data
{
    public class GerenciamentoTarefasContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<HistoricoAlteracao> HistoricoAlteracoes { get; set; }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Tarefa &&
                            (e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var tarefa = entry.Entity as Tarefa;
                var projeto = Projetos.Find(tarefa.ProjetoId);
               
                if (entry.State == EntityState.Modified)
                {
                    var originalValues = entry.OriginalValues;
                    var currentValues = entry.CurrentValues;

                    var alteracoes = new List<string>();
                    foreach (var property in originalValues.Properties)
                    {
                        var original = originalValues[property]?.ToString();
                        var atual = currentValues[property]?.ToString();

                        if (original != atual)
                        {
                            alteracoes.Add($"{property.Name}: '{original}' -> '{atual}'");
                        }
                    }

                    HistoricoAlteracoes.Add(new HistoricoAlteracao
                    {
                        TarefaId = tarefa.Id,
                        UserId = projeto.UserId,
                        DataAlteracao = DateTime.UtcNow,
                        Alteracoes = string.Join("; ", alteracoes)
                    });
                }
            }

            return base.SaveChanges();
        }
        public GerenciamentoTarefasContext(DbContextOptions<GerenciamentoTarefasContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
                entity.HasMany(u => u.Projetos)
                      .WithOne(p => p.Usuario)
                      .HasForeignKey(p => p.UserId);
            });

            modelBuilder.Entity<Projeto>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nome).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Descricao).HasMaxLength(500);
                entity.HasMany(p => p.Tarefas)
                      .WithOne()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Descricao).HasMaxLength(1000);
                entity.Property(t => t.DataVencimento).IsRequired();
                entity.Property(t => t.Status).IsRequired();
                entity.Property(t => t.Prioridade).IsRequired();
                entity.HasMany(t => t.Comentarios)
                      .WithOne(t => t.Tarefa)
                      .HasForeignKey(h => h.TarefaId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(t => t.HistoricoAlteracoes)
                      .WithOne(h => h.Tarefa)
                      .HasForeignKey(h => h.TarefaId);
                entity.HasOne(t => t.Projeto)
                .WithMany(p => p.Tarefas)
                .HasForeignKey(t => t.ProjetoId);

            });

            modelBuilder.Entity<HistoricoAlteracao>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.DataAlteracao).IsRequired();
                entity.Property(h => h.Alteracoes);
                entity.HasOne(h => h.Usuario)
                      .WithMany()
                      .HasForeignKey(h => h.UserId);
            });

            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Texto).IsRequired().HasMaxLength(500);
            });
        }

        public void SeedData()
        {
            if (!Usuarios.Any())
            {
                Usuarios.Add(new Usuario
                {
                    UserName = "admin"
                });
                SaveChanges();
            }
        }
    }
}
