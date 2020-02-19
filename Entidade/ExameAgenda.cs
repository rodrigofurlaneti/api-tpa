using Entidade.Base;
using System;

namespace Entidade
{
    public class ExameAgenda : BaseEntity
    {
        public ExameAgenda()
        {
            DataInsercao = DateTime.Now;
        }
        
        public virtual Pessoa Paciente { get; set; }

        public virtual Exame Exame { get; set; }

        public virtual DateTime DataHoraExame { get; set; }

        public virtual Clinica Clinica { get; set; }
    }
}