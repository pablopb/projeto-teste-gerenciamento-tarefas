using GerenciamentoTarefas.Domain.Entity;

namespace GerenciamentoTarefasApi.DTOs
{
    public class ProjetoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int UserId { get; set; }
    }
}
