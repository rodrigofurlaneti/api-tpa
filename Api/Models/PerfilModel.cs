using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class PerfilModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public PerfilModel FromEntity(Perfil perfil)
        {
            Id = perfil.Id;
            Nome = perfil.Nome;

            return this;
        }

        public Perfil ToEntity()
        {
            return new Perfil
            {
                Id = Id,
                Nome = Nome
            };
        }
    }
}