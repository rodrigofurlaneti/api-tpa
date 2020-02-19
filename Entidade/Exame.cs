using Entidade.Base;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class Exame : BaseEntity
    {
        public Exame()
        {
            DataInsercao = DateTime.Now;
        }

        public virtual string Nome { get; set; }

        public virtual IList<Preco> Precos { get; set; }

        //TODO: Criar IList<ExameVaga>
    }
}