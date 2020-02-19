using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio
{
    public class ClinicaRepositorio : NHibRepository<Clinica>, IClinicaRepositorio
    {
        public ClinicaRepositorio(NHibContext context) 
            : base(context)
        {
        }
    }
}
