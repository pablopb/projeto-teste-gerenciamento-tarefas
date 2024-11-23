using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciamentoTarefas.Domain.Entity
{
    public class Tarefa
    {
        public int Id { get; set; } 
        public virtual ICollection<Comentario>? Comentarios { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public StatusTarefa Status { get; set; }
        public PrioridadeTarefa Prioridade { get; set; }
        public ICollection<HistoricoAlteracao>? HistoricoAlteracoes { get; set; }
        public int ProjetoId { get; set; }
        public virtual Projeto Projeto { get; set; }
        public int UserId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
