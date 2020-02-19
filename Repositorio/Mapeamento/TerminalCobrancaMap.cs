using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TerminalCobrancaMap : ClassMap<TerminalCobranca>
    {
        public TerminalCobrancaMap()
        {
            Table("TerminalCobranca");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DataInsercao).Not.Update();
            Map(x => x.EnderecoIP);
            Map(x => x.EnderecoMAC);
            Map(x => x.Modelo);
            Map(x => x.NumeroSerial);
            Map(x => x.TipoTerminal);

            Map(x => x.Maquininha).Column("Maquininha");
            Map(x => x.NomeGerenciadora).Column("NomeGerenciadora");
            Map(x => x.TaxaRefeicao).Column("TaxaRefeicao");
            Map(x => x.TaxaAlimentacao).Column("TaxaAlimentacao");
            Map(x => x.TaxaCombustivel).Column("TaxaCombustivel");
            Map(x => x.TaxaAdiantamento).Column("TaxaAdiantamento");
        }
    }
}
