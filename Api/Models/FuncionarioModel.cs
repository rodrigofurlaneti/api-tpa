using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class FuncionarioModel : PessoaModel
    {
        public string MatriculaEmpresa { get; set; }
        public string Cargo { get; set; }
        public UsuarioModel Usuario { get; set; }

        public FuncionarioModel() { }

        public FuncionarioModel(Funcionario funcionario) : base(funcionario) { }

        public FuncionarioModel FromEntity(Funcionario entity)
        {
            MatriculaEmpresa = entity.MatriculaEmpresa;
            Cargo = entity.Cargo;
            Usuario = entity.Usuario != null ? new UsuarioModel(entity.Usuario) : null;

            return this;
        }

        public Funcionario ToEntity()
        {
            var funcionario = base.ToEntity<Funcionario>();
            funcionario.MatriculaEmpresa = this.MatriculaEmpresa;
            funcionario.Cargo = this.Cargo;
            funcionario.Usuario = this.Usuario?.ToEntity();
            if (funcionario.Usuario != null) funcionario.Usuario.Pessoa = funcionario;
            return funcionario;
        }
    }
}