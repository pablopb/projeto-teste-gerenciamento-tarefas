namespace GerenciamentoTarefas.Domain.Entity
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public virtual ICollection<Tarefa>? Tarefas { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int UserId { get; set; }

    }
}
