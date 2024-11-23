using GerenciamentoTarefas.Domain.Entity;

namespace GerenciamentoTarefasApi.DTOs
{
    public class HistoricoAlteracaoDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Alteracoes { get; set; }
        public int TarefaId { get; set; }
        public int UserId { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
