using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class ClinicaMap : SubclassMap<Clinica>
    {
        public ClinicaMap()
        {
            Map(x => x.GaleriaFotos);
            Map(x => x.PossuiAcessibilidadePCD);
            References(x => x.ContaFinanceira).Cascade.All();
            HasManyToMany(x => x.GaleriaFotos).Cascade.All();
        }
    }
}
