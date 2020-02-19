using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class ExameAgendaMap : ClassMap<ExameAgenda>
    {
        public ExameAgendaMap()
        {
            Id(x => x.Id);
            Map(x => x.DataInsercao);
            Map(x => x.DataHoraExame);

            References(x => x.Clinica);
            References(x => x.Paciente);
            References(x => x.Exame);
        }
    }
}
