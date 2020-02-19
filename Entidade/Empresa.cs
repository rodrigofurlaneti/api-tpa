using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entidade
{
    public class Empresa : Pessoa
    {
        public Empresa()
        {
            DataInsercao = DateTime.Now;
        }

        public virtual Imagem Imagem { get; set; }
        public virtual int NumeroFuncionarios { get; set; }
        public virtual Funcionario Responsavel { get; set; }
        public virtual IList<Funcionario> Funcionarios { get; set; }
        
        public virtual string Telefone {
            get {
                return Contatos?.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Comercial)?.Contato.Numero;
            }
        }

    }
}