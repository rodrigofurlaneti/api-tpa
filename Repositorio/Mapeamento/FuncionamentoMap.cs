using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Repositorio.Mapeamento
{
    public class FuncionamentoMap : SubclassMap<Funcionamento>
    {
        public FuncionamentoMap()
        {
            Map(x => x.DiaFuncionamento);
            Map(x => x.ManhaInicio);
            Map(x => x.ManhaFim);
            Map(x => x.TardeInicio);
            Map(x => x.TardeFim);
        }
    }
}
