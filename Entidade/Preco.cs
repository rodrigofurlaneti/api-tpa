using Entidade.Base;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class Preco : BaseEntity
    {
        public Preco()
        {
            DataInsercao = DateTime.Now;
        }

        public virtual Exame Exame { get; set; }

        public virtual decimal Valor { get; set; }
    }
}