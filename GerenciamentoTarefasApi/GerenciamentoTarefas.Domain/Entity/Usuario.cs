namespace GerenciamentoTarefas.Domain.Entity
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Perfil Perfil { get; set; }
        public ICollection<Projeto>? Projetos { get; set; }
    }
}
