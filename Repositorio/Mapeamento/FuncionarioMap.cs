using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class FuncionarioMap : SubclassMap<Funcionario>
    {
        public FuncionarioMap()
        {
            Map(x => x.MatriculaEmpresa);
            Map(x => x.Cargo);
            Map(x => x.Setor);
            Map(x => x.Filial);

            References(x => x.Empresa).Cascade.SaveUpdate();
            References(x => x.Usuario).Cascade.All();
            HasMany(x => x.Exames).Cascade.All();
        }
    }
}
