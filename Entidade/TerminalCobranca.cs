using Entidade.Base;

namespace Entidade
{
    public class TerminalCobranca : BaseEntity
    {
        public TerminalCobranca()
            :base()
        {

        }

        public virtual string EnderecoIP { get; set; }
        public virtual string EnderecoMAC { get; set; }
        public virtual string Modelo { get; set; }
        public virtual string NumeroSerial { get; set; }
        public virtual int TipoTerminal { get; set; }

        public virtual string Maquininha { get; set; }
        public virtual string NomeGerenciadora { get; set; }
        public virtual decimal TaxaRefeicao { get; set; }
        public virtual decimal TaxaAlimentacao { get; set; }
        public virtual decimal TaxaCombustivel { get; set; }
        public virtual decimal TaxaAdiantamento { get; set; }
    }
}
