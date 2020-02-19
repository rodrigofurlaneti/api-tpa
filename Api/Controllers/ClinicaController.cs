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
    [Authorize, RoutePrefix("api/clinica")]
    public class ClinicaController : ApiController
    {
        private readonly IClinicaServico clinicaServico;
        private readonly IUsuarioServico usuarioServico;
        private readonly IFuncionarioServico funcionarioServico;
        public ClinicaController(IClinicaServico clinicaServico, IUsuarioServico usuarioServico, IFuncionarioServico funcionarioServico)
        {
            this.clinicaServico = clinicaServico;
            this.usuarioServico = usuarioServico;
            this.funcionarioServico = funcionarioServico;
        }

        [HttpPost, AllowAnonymous, Route("cadastrar")]
        [SwaggerResponse(HttpStatusCode.Created, "Clinica cadastrada", typeof(ClinicaModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Dados inválidos")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno")]
        public ClinicaModel Cadastrar(ClinicaModel clinicaModel)
        {
            clinicaModel.Validar();

            var clinica = clinicaModel.ToEntity();
            clinicaServico.Salvar(clinica);

            clinica = clinicaServico.BuscarPorId(clinica.Id);

            var senha = Tools.GenerateRandomValue();
            var usuario = usuarioServico.Register(clinica.Responsavel, senha, clinica.Responsavel.Usuario.Id);

            var body = $"Olá, foi adicionado a clinica \"{clinica.Nome}\". <br />";
            body += $"Seus dados de login são:<br />";
            body += $"Login: {clinica.Responsavel.Email}<br />";
            body += $"Senha: {senha}<br />";
            Mail.SendMail(clinica.Responsavel.Email, "[OCUP - APP] - Clínica cadastrada.", body, "administrador@4world.com.br");

            return new ClinicaModel().FromEntity(clinica);
        }

        [HttpPut, Route("{id}/alterar")]
        public ClinicaModel Alterar(int id, [FromBody]ClinicaModel clinicaModel)
        {
            var clinica = clinicaModel.ToEntity();
            clinica.Id = id;
            clinicaServico.Salvar(clinica);

            return new ClinicaModel().FromEntity(clinica);
        }

        [HttpGet, Route("{id}")]
        public ClinicaModel GetId(int id)
        {
            var clinica = clinicaServico.BuscarPorId(id);
            var clinicaModel = new ClinicaModel().FromEntity(clinica);
            return clinicaModel;
        }

        [HttpGet, Route("{id}/usuarios")]
        public UsuarioModel[] Usuarios(int id)
        {
            var usuarios = clinicaServico.BuscarPorId(id)?.Funcionarios.Select(x => x.Usuario).AsQueryable();

            return Request.PagedResult(usuarios).Select(x => new UsuarioModel(x)).ToArray();
        }

        [HttpGet, Route("{id}/usuarios/{perfil}")]
        public UsuarioModel[] UsuariosPerfil(int id, string perfil)
        {
            var usuarios = clinicaServico
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
            var empresa = clinicaServico.BuscarPorId(id);
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

            var body = $"Olá, você foi adicionado como funcionário pela clínica \"{empresa.Nome}\". <br />";
            body += $"Seus dados de login são:<br />";
            body += $"Login: {funcionario.Email}<br />";
            body += $"Senha: {senha}<br />";
            Mail.SendMail(funcionario.Email, "[OCUP - APP] - Usuário cadastrado.", body, "administrador@4world.com.br");

            return new UsuarioModel(usuario);
        }

        [HttpDelete, Route("{id}/usuario/{userId}/excluir")]
        public void ExcluirUsuario(int id, int userId)
        {
            var empresa = clinicaServico.BuscarPorId(id);
            var funcionario = empresa.Funcionarios.FirstOrDefault(f => f.Usuario.Id == userId);

            funcionarioServico.Excluir(funcionario);
        }

        [HttpPost, Route()]
        public HttpResponseMessage Pesquisar(FiltrosPesquisaModel filtros)
        {


            return Request.CreateResponse(HttpStatusCode.OK, new ClinicaModel[] { });
        }
    }
}
