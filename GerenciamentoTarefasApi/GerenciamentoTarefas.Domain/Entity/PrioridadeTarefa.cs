using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciamentoTarefas.Domain.Entity
{
    public enum PrioridadeTarefa
    {
        [Description("Baixa")]
        BAIXA = 1,
        [Description("Média")]
        MEDIA = 2,
        [Description("Alta")]
        ALTA = 3
    }
}
