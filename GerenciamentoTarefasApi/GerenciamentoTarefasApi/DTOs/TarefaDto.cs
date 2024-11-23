using GerenciamentoTarefas.Domain.Entity;

namespace GerenciamentoTarefasApi.DTOs
{
    public class TarefaDto
    {
        public int Id { get; set; }
        public virtual ICollection<ComentarioDto>? Comentarios { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public StatusTarefa Status { get; set; }
        public PrioridadeTarefa Prioridade { get; set; }
        public ICollection<HistoricoAlteracaoDto>? HistoricoAlteracoes { get; set; }
        public int ProjetoId { get; set; }
        public int UserId { get; set; }
    }
}
