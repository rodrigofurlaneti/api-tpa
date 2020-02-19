using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class ExameMap : ClassMap<Exame>
    {
        public ExameMap()
        {
            Id(x => x.Id);
            Map(x => x.DataInsercao);
            Map(x => x.Nome);
            HasMany(x => x.Precos);
        }
    }
}
