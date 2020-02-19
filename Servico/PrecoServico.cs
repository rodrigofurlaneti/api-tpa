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
    public interface IPrecoServico : IBaseServico<Preco>
    {
    }

    public class PrecoServico : BaseServico<Preco, IPrecoRepositorio>, IPrecoServico
    {
        private readonly IPrecoRepositorio _PrecoRepositorio;

        public PrecoServico(IPrecoRepositorio PrecoRepositorio)
        {
            _PrecoRepositorio = PrecoRepositorio;
        }
    }
}