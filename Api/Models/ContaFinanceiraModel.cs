using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class ContaFinanceiraModel
    {
        public int Id { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string Descricao { get; set; }
        public string Banco { get; set; }
        public string Convenio { get; set; }
        public string Carteira { get; set; }
        public string CodigoTransmissao { get; set; }
        public string ContaPadrao { get; set; }
        public string ConvenioPagamento { get; set; }
        
        public ContaFinanceiraModel FromEntity(ContaFinanceira entity)
        {
            Id = entity.Id;
            return this;
        }

        public ContaFinanceira ToEntity()
        {
            ContaFinanceira contaFinanceira = null;
            contaFinanceira = new ContaFinanceira()
            {
                Id = Id,
            };
            return contaFinanceira;
        }

        public void Validar()
        {

        }
    }
}