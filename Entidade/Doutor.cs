using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace Entidade
{
    public class Doutor : Pessoa
    {
        public Doutor()
        {
            DataInsercao = DateTime.Now;
            DataNascimento = SqlDateTime.MinValue.Value;
        }
        
        public virtual string CRM { get; set; }

        public virtual IList<Clinica> Clinicas { get; set; }
    }
}