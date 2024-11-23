namespace GerenciamentoTarefasApi.DTOs
{
    public class ComentarioDto
    {
        public int Id { get; set; } 
        public string Texto { get; set; }
        public int TarefaId { get; set; }
    }
}
