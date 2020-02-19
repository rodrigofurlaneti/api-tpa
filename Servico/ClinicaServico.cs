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
    public interface IClinicaServico : IBaseServico<Clinica>
    {
    }

    public class ClinicaServico : BaseServico<Clinica, IClinicaRepositorio>, IClinicaServico
    {
        private readonly IClinicaRepositorio _ClinicaRepositorio;

        public ClinicaServico(IClinicaRepositorio ClinicaRepositorio)
        {
            _ClinicaRepositorio = ClinicaRepositorio;
        }
    }
}