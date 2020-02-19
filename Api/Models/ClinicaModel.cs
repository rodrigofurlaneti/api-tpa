using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class ClinicaModel:EmpresaModel
    {
        public string Endereco { get; set; }
        
        public ClinicaModel FromEntity(Clinica entity)
        {
            base.FromEntity(entity);
            //Id = entity.Id;
            //Cnpj = entity.Cnpj;
            //NomeClinica = entity.NomeClinica;
            //NomeResponsavel = entity.NomeResponsavel;
            //EmailResponsavel = entity.EmailResponsavel;
            //Telefone = entity.Telefone;
            //Endereco = entity.Endereço;
            return this;
        }

        public Clinica ToEntity()
        {
            Clinica clinica = base.ToEntity<Clinica>();
            return clinica;
        }

        public void Validar()
        {
            this.Responsavel.Sexo = "I";
        }
    }
}