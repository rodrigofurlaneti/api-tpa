using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Entidade.Base;

namespace Entidade
{
    public class PlanoVenda : IEntity
    {
        public PlanoVenda()
        {
            DataInsercao = DateTime.Now;
        }

        public virtual int Id { get; set; }
        public virtual DateTime DataInsercao { get; set; }

        [Required, StringLength(2000)]
        public virtual string Nome { get; set; }
        [Required, StringLength(2000)]
        public virtual string Descricao { get; set; }
        
        public virtual string Foto { get; set; }
        
        public virtual decimal Valor { get; set; }

        public virtual decimal ValorDesconto { get; set; }

        public virtual DateTime InicioVigencia { get; set; }

        public virtual DateTime FimVigencia { get; set; }

        public virtual bool Status { get; set; }

        public virtual Convenio Convenio { get; set; }
    }
}