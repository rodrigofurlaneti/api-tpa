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
    public interface IExameServico : IBaseServico<Exame>
    {
    }

    public class ExameServico : BaseServico<Exame, IExameRepositorio>, IExameServico
    {
        private readonly IExameRepositorio _ExameRepositorio;

        public ExameServico(IExameRepositorio ExameRepositorio)
        {
            _ExameRepositorio = ExameRepositorio;
        }
    }
}