using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Models
{
    public class UsuarioModel
    {
        public UsuarioModel() { }

        public UsuarioModel(Usuario usuario)
        {
            if (usuario == null)
                throw new Exception("O usuario não foi encontrado.");

            var usuarioPerfil = usuario.Perfils?.FirstOrDefault();

            Id = usuario.Id;
            Login = usuario.Login;
            Pessoa = usuario.Pessoa != null ? new PessoaModel(usuario.Pessoa) : null;
            Perfil = usuarioPerfil != null ? new PerfilModel().FromEntity(usuarioPerfil.Perfil) : null;
        }

        public PessoaModel Pessoa { get; set; }
        public int Id { get; set; }
        public PerfilModel Perfil { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }

        public Usuario ToEntity()
        {
            var usuario = new Usuario {
                Id = this.Id,
                Login = this.Login,
                Senha = this.Senha,
                Perfils = new List<UsuarioPerfil>()
            };

            if (this.Perfil != null) usuario.Perfils.Add(new UsuarioPerfil { Perfil = this.Perfil?.ToEntity() });

            return usuario;
        }
    }
}