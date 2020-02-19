using Entidade.Uteis;
using System;

namespace Entidade
{
    public class PrecoAgenda : Preco
    {
        public PrecoAgenda()
        {
            DataInsercao = DateTime.Now;
        }

        public virtual DiaSemanal DiaSemanal { get; set; }

        public virtual PeriodoDia PeriodoDia { get; set; }
    }
}