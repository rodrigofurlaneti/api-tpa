using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class EmpresaModel
    {
        /// <summary>
        /// Id será retornado se a empresa for cadastrada
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Documento da empresa
        /// </summary>
        public string Cnpj { get; set; }
        /// <summary>
        /// Nome da empresa
        /// </summary>
        public string RazaoSocial { get; set; }
        /// <summary>
        /// Telefone da empresa
        /// </summary>
        public string Telefone { get; set; }
        public int NumeroFuncionarios { get; set; }
        public FuncionarioModel Responsavel { get; set; }

        public EmpresaModel FromEntity(Empresa entity)
        {
            Id = entity.Id;
            Cnpj = entity.Cnpj;
            RazaoSocial = entity.Nome;
            Telefone = entity.Telefone;
            NumeroFuncionarios = entity.NumeroFuncionarios;
            Responsavel = new FuncionarioModel(entity.Responsavel);
            return this;
        }

        public T ToEntity<T>() where T : Empresa, new()
        {
            var empresa = new T();
            empresa.Id = Id;
            empresa.Nome = RazaoSocial;
            empresa.Sexo = "I";
            empresa.NumeroFuncionarios = NumeroFuncionarios;
            empresa.Documentos = new List<Documento>
            {
                new Documento { Tipo = TipoDocumento.Cnpj, Numero = Cnpj }
            };
            empresa.Contatos = new List<PessoaContato>
            {
                new PessoaContato {
                    Contato = new Contato
                    {
                        Tipo = TipoContato.Comercial,
                        Numero = Telefone,
                        NomeRecado = Responsavel?.Nome
                    }
                }
            };
            empresa.Responsavel = Responsavel?.ToEntity();
            if (empresa.Responsavel != null) empresa.Responsavel.Empresa = empresa;
            return empresa;
        }

        public void Validar()
        {
            this.Responsavel.Sexo = "I";
        }
    }
}