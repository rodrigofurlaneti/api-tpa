using Entidade.Base;
using Entidade.Uteis;
using System;
namespace Entidade
{
    public class Funcionamento : BaseEntity
    {
        public Funcionamento()
        {
            DataInsercao = DateTime.Now;
        }
        public virtual DiaFuncionamento DiaFuncionamento { get; set; }
        public virtual string ManhaInicio { get; set; }
        public virtual string ManhaFim { get; set; }
        public virtual string TardeInicio { get; set; }
        public virtual string TardeFim { get; set; }
    }
}
