using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciamentoTarefas.Domain.Entity
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public Tarefa Tarefa { get; set; }
        public int TarefaId { get; set; }
    }
}
