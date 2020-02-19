using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PessoaMap : ClassMap<Pessoa>
    {
        public PessoaMap()
        {
            Table("Pessoa");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.DataNascimento).Column("DataNascimento");
            Map(x => x.Sexo).Column("Sexo").Not.Nullable();
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            
            HasMany(x => x.Documentos).Cascade.All();

            HasMany(x => x.Contatos)
                .Table("PessoaContato")
                .KeyColumn("Pessoa")
                .Component(m =>
                {
                    m.References(x => x.Contato).Cascade.All();
                }).Cascade.All();
            
            HasMany(x => x.Enderecos).Cascade.All();
        }
    }
}