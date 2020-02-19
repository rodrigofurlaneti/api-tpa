using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class PrecoMap : ClassMap<Preco>
    {
        public PrecoMap()
        {
            Id(x => x.Id);
            Map(x => x.DataInsercao);
            Map(x => x.Valor);

            References(x => x.Exame);
        }
    }
}
