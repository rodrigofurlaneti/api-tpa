using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public interface IImagemServico : IBaseServico<Imagem>
    {
    }

    public class ImagemServico : BaseServico<Imagem, IImagemRepositorio>, IImagemServico
    {
        private readonly IImagemRepositorio _ImagemRepositorio;

        public ImagemServico(IImagemRepositorio ImagemRepositorio)
        {
            _ImagemRepositorio = ImagemRepositorio;
        }
    }
}