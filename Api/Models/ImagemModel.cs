using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class ImagemModel
    {
        public int Id { get; set; }
        public string DataInsercao { get; set; }
        public string ImagemUpload { get; set; }

        public ImagemModel FromEntity(Imagem entity)
        {
            Id = entity.Id;
            return this;
        }

        public Imagem ToEntity()
        {
            Imagem imagem = null;
            imagem = new Imagem()
            {
                Id = Id,
            };
            return imagem;
        }

        public void Validar()
        {

        }
    }
}