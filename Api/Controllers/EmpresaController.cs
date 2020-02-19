using Api.Base;
using Api.Helpers;
using Api.Models;
using Core;
using Core.Extensions;
using Dominio;
using Dominio.Base;
using Entidade;
using Entidade.Uteis;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Api.Controllers
{
    [Authorize, RoutePrefix("api/empresa")]
    public class EmpresaController : ApiController
    {
        private readonly IEmpresaServico empresaServico;
        private readonly IContatoServico contatoServico;
        private readonly IDocumentoServico documentoServico;
        private readonly IUsuarioServico usuarioServico;
        private readonly IFuncionarioServico funcionarioServico;

        public EmpresaController(
            IEmpresaServico empresaServico,
            IContatoServico contatoServico,
            IDocumentoServico documentoServico,
            IUsuarioServico usuarioServico,
            IFuncionarioServico funcionarioServico
        )
        {
            this.empresaServico = empresaServico;
            this.contatoServico = contatoServico;
            this.documentoServico = documentoServico;
            this.usuarioServico = usuarioServico;
            this.funcionarioServico = funcionarioServico;
        }

        /// <summary>
        /// Cadastro de empresa mais simplificado
        /// </summary>
        /// <param name="empresaModel">Empresa para ser cadastrada</param>
        /// <returns>
        /// Empresa cadastrada com Id
        /// </returns>
        [HttpPost, AllowAnonymous, Route("cadastrar")]
        [SwaggerResponse(HttpStatusCode.Created, "Empresa cadastrada", typeof(EmpresaModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Dados inválidos")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno")]
        public EmpresaModel Cadastrar(EmpresaModel empresaModel)
        {
            empresaModel.Validar();
            
            var empresa = empresaModel.ToEntity<Empresa>();
            empresaServico.Salvar(empresa);

            empresa = empresaServico.BuscarPorId(empresa.Id);

            var senha = Tools.GenerateRandomValue();
            var usuario = usuarioServico.Register(empresa.Responsavel, senha, empresa.Responsavel.Usuario.Id);

            var body = $"Olá, você foi adicionado como responsável pela empresa \"{empresa.Nome}\". <br />";
            body += $"Seus dados de login são:<br />";
            body += $"Login: {empresa.Responsavel.Email}<br />";
            body += $"Senha: {senha}<br />";
            Mail.SendMail(empresa.Responsavel.Email, "[OCUP - APP] - Usuário cadastrado.", body, "administrador@4world.com.br");
            
            return new EmpresaModel().FromEntity(empresa);
        }

        [HttpPut, Route("{id}/alterar")]
        public EmpresaModel Alterar (int id, [FromBody]EmpresaModel empresaModel)
        {
            var empresa = empresaModel.ToEntity<Empresa>();
            empresa.Id = id;
            empresaServico.Salvar(empresa);

            return new EmpresaModel().FromEntity(empresa);
        }

        [HttpGet, Route("{id}")]
        public EmpresaModel GetId(int id)
        {
            var empresa = empresaServico.BuscarPorId(id);
            var empresaModel = new EmpresaModel().FromEntity(empresa);
            return empresaModel;
        }

        [HttpGet, Route("{id}/usuarios")]
        public UsuarioModel[] Usuarios(int id)
        {
            var usuarios = empresaServico.BuscarPorId(id)?.Funcionarios.Select(x => x.Usuario).AsQueryable();
            
            return Request.PagedResult(usuarios).Select(x => new UsuarioModel(x)).ToArray();
        }

        [HttpGet, Route("{id}/usuarios/{perfil}")]
        public UsuarioModel[] UsuariosPerfil(int id, string perfil)
        {
            var usuarios = empresaServico
                .BuscarPorId(id)?
                .Funcionarios
                .Select(x => x.Usuario)
                .Where(u =>
                    u.Perfils.Any(p => p.Perfil.Nome.ToLower().Equals(perfil.ToLower()))
                )
                .AsQueryable();

            return Request.PagedResult(usuarios).Select(x => new UsuarioModel(x)).ToArray();
        }

        [HttpPost, Route("{id}/usuarios/inserir")]
        public UsuarioModel InserirUsuario(int id, [FromBody]UsuarioModel usuarioModel)
        {
            var empresa = empresaServico.BuscarPorId(id);
            var funcionario = new Funcionario
            {
                Nome = usuarioModel.Pessoa.Nome,
                Sexo = "I",
                Empresa = empresa,
                Contatos = new List<PessoaContato>
                {
                    new PessoaContato { Contato = new Contato { Tipo = TipoContato.Email, Email = usuarioModel.Pessoa.Email} }
                }
            };
            funcionarioServico.Salvar(funcionario);

            var senha = Tools.GenerateRandomValue();
            var usuario = usuarioServico.Register(funcionario, senha, 0, null, usuarioModel.Perfil.Id);

            funcionario.Usuario = usuario;
            funcionarioServico.Salvar(funcionario);

            var body = $"Olá, você foi adicionado como funcionário pela empresa \"{empresa.Nome}\". <br />";
            body += $"Seus dados de login são:<br />";
            body += $"Login: {funcionario.Email}<br />";
            body += $"Senha: {senha}<br />";
            Mail.SendMail(funcionario.Email, "[OCUP - APP] - Usuário cadastrado.", body, "administrador@4world.com.br");
            
            return new UsuarioModel(usuario);
        }

        [HttpDelete, Route("{id}/usuario/{userId}/excluir")]
        public void ExcluirUsuario(int id, int userId)
        {
            var empresa = empresaServico.BuscarPorId(id);
            var funcionario = empresa.Funcionarios.FirstOrDefault(f => f.Usuario.Id == userId);
            
            funcionarioServico.Excluir(funcionario);
        }
    }
}
