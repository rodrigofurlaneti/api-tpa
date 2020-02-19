using Entidade.Base;
using System;
using System.Linq;

namespace Entidade
{
    public class Imagem : BaseEntity
    {
        public Imagem()
        {
            DataInsercao = DateTime.Now;
        }

        public virtual byte[] ImagemUpload { get; set; }

        public virtual string GetImage()
        {
            return ImagemUpload != null && ImagemUpload.Any()
                        ? $"data:image/jpg;base64,{Convert.ToBase64String(ImagemUpload)}"
                        : string.Empty;
        }
    }
}