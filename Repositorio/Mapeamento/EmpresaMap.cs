using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class EmpresaMap : SubclassMap<Empresa>
    {
        public EmpresaMap()
        {
            Map(x => x.NumeroFuncionarios);

            References(x => x.Responsavel).Cascade.All();
            HasMany(x => x.Funcionarios).Cascade.All();
        }
    }
}
