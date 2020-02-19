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
    [Authorize, RoutePrefix("api/exame")]
    public class ExameController : ApiController
    {
        private readonly IExameServico exameServico;

        public ExameController(IExameServico exameServico)
        {
            this.exameServico = exameServico;
        }

        [HttpPost, AllowAnonymous, Route("cadastrar")]
        [SwaggerResponse(HttpStatusCode.Created, "Exame cadastrado", typeof(ExameModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Dados inválidos")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno")]
        public ExameModel Cadastrar(ExameModel exameModel)
        {
            exameModel.Validar();

            var exame = exameModel.ToEntity();
            exameServico.Salvar(exame);

            exame = exameServico.BuscarPorId(exame.Id);

            var body = $"Olá, foi adicionado o exame \"{exame.Nome}\". <br />";
            body += $"Nome do exame: {exame.Nome}<br />";

            return new ExameModel().FromEntity(exame);
        }

        [HttpPut, Route("{id}/alterar")]
        public ExameModel Alterar(int id, [FromBody]ExameModel exameModel)
        {
            var exame = exameModel.ToEntity();
            exame.Id = id;
            exameServico.Salvar(exame);

            return new ExameModel().FromEntity(exame);
        }

        [HttpGet, Route("{id}")]
        public ExameModel GetId(int id)
        {
            var exame = exameServico.BuscarPorId(id);
            var exameModel = new ExameModel().FromEntity(exame);
            return exameModel;
        }

        [HttpPost, Route()]
        public HttpResponseMessage Pesquisar(FiltrosPesquisaModel filtros)
        {


            return Request.CreateResponse(HttpStatusCode.OK, new ExameModel[] { });
        }
    }
}