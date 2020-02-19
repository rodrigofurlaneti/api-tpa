using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entidade;
namespace Api.Models
{
    public class FuncionamentoModel
    {
        public int Id { get; set; }
        public string DiaFuncionamento { get; set; }
        public string ManhaInicio { get; set; }
        public string ManhaFim { get; set; }
        public string TardeInicio { get; set; }
        public string TardeFim { get; set; }

        public FuncionamentoModel FromEntity(Funcionamento entity)
        {
            Id = entity.Id;
            return this;
        }
        public Funcionamento ToEntity()
        {
            Funcionamento funcionamento = null;
            funcionamento = new Funcionamento()
            {
                Id = Id,
            };
            return funcionamento;
        }
        public void Validar()
        {
        }
    }
}