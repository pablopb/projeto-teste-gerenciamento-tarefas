namespace GerenciamentoTarefas.Domain.Entity
{
    public class HistoricoAlteracao
    {
        public int Id { get; set; }
        public int TarefaId { get; set; }
        public Tarefa Tarefa { get; set; }
        public int UserId { get; set; }
        public  Usuario Usuario {get; set;}
        public DateTime DataAlteracao { get; set; }
        public string Alteracoes { get; set; }
    }
}
