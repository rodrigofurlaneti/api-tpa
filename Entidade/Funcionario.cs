using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace Entidade
{
    public class Funcionario : Pessoa
    {
        public Funcionario()
        {
            DataInsercao = DateTime.Now;
            DataNascimento = SqlDateTime.MinValue.Value;
        }

        public virtual string MatriculaEmpresa { get; set; }
        public virtual string Cargo { get; set; }
        public virtual string Setor { get; set; }
        public virtual string Filial { get; set; }

        public virtual Empresa Empresa { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual IList<ExameAgenda> Exames { get; set; }
    }
}