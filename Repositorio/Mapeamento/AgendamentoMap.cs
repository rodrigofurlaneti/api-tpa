﻿using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class AgendamentoMap : ClassMap<Agendamento>
    {
        public AgendamentoMap()
        {
            Table("Agendamento");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Data).Column("Data");
            Map(x => x.CapacidadeDeAtendimento).Column("CapacidadeDeAtendimento");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            
            //HasMany(x => x.ItensCompra)
            //    .Table("ItemCompra")
            //    .KeyColumn("Agendamento")
            //    //.Component(m =>
            //    //{
            //    //    m.Map(x => x.Agendamento).Column("Agendamento");
            //    //})
            //    .Inverse();
        }
    }
}