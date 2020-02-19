using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class PrecoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Precos { get; set; }

        public PrecoModel FromEntity(Preco entity)
        {
            Id = entity.Id;
            return this;
        }

        public Preco ToEntity()
        {
            Preco Preco = null;
            Preco = new Preco()
            {
                Id = Id,
            };
            return Preco;
        }

        public void Validar()
        {

        }
    }
}