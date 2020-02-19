using System;
using System.Collections.Generic;
using Entidade;
using Entidade.Uteis;

namespace Api.Models
{
    public class PessoaModel
    {
        public PessoaModel()
        {
            
        }

        public PessoaModel(Pessoa pessoa)
        {
            if (pessoa == null)
                throw new Exception("A pessoa não foi encontrado.");

            Id = pessoa.Id;
            Nome = pessoa.Nome;
            Sexo = pessoa.Sexo;
            DataNascimento = pessoa.DataNascimento;
            Cpf = pessoa.Cpf;
            Rg = pessoa.Rg;
            Celular = pessoa.Celular;
            Email = pessoa.Email;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }

        public T ToEntity<T>() where T : Pessoa, new()
        {
            var pessoa = new T();
            pessoa.Nome = this.Nome;
            pessoa.Sexo = this.Sexo;
            pessoa.DataNascimento = this.DataNascimento;
            pessoa.Contatos = new List<PessoaContato>();

            if (!string.IsNullOrEmpty(Rg)) pessoa.Documentos.Add(new Documento { Tipo = TipoDocumento.Rg, Numero = this.Rg });
            if (!string.IsNullOrEmpty(Cpf)) pessoa.Documentos.Add(new Documento { Tipo = TipoDocumento.Cpf, Numero = this.Cpf });
            if (!string.IsNullOrEmpty(Email)) pessoa.Contatos.Add(new PessoaContato { Contato = new Contato { Tipo = TipoContato.Email, Email = this.Email } });
            if (!string.IsNullOrEmpty(Celular)) pessoa.Contatos.Add(new PessoaContato { Contato = new Contato { Tipo = TipoContato.Celular, Numero = this.Celular } });

            return pessoa;
        }
    }
}