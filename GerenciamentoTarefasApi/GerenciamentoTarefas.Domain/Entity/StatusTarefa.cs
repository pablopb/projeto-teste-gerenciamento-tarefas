using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciamentoTarefas.Domain.Entity
{
    public enum StatusTarefa
    {
        [Description("Pendente")]
        PENDENTE = 1,
        [Description("Em Andamento")]
        EM_ANDAMENTO = 2,
        [Description("Concluída")]
        CONCLUIDA = 3
    }
}
