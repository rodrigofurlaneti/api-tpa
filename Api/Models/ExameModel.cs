using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class ExameModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Precos { get; set; }

        public ExameModel FromEntity(Exame entity)
        {
            Id = entity.Id;
            return this;
        }

        public Exame ToEntity()
        {
            Exame exame = null;
            exame = new Exame()
            {
                Id = Id,
            };
            return exame;
        }

        public void Validar()
        {

        }
    }
}