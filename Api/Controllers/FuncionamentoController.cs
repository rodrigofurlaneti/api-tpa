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
    [Authorize, RoutePrefix("api/funcionamento")]
    public class FuncionamentoController : ApiController
    {
        private readonly IFuncionamentoServico funcionamentoServico;

        public FuncionamentoController(IFuncionamentoServico funcionamentoServico)
        {
            this.funcionamentoServico = funcionamentoServico;
        }

        [HttpPost, AllowAnonymous, Route("cadastrar")]
        [SwaggerResponse(HttpStatusCode.Created, "Funcionamento cadastrado", typeof(FuncionamentoModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Dados inválidos")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno")]
        public FuncionamentoModel Cadastrar(FuncionamentoModel funcionamentoModel)
        {
            funcionamentoModel.Validar();

            var funcionamento = funcionamentoModel.ToEntity();
            funcionamentoServico.Salvar(funcionamento);

            funcionamento = funcionamentoServico.BuscarPorId(funcionamento.Id);

            var body = $"Olá, foi adicionado o funcionamento \"{funcionamento.DiaFuncionamento}\". <br />";
            body += $"Nome do funcionamento: {funcionamento.DiaFuncionamento}<br />";

            return new FuncionamentoModel().FromEntity(funcionamento);
        }

        [HttpPut, Route("{id}/alterar")]
        public FuncionamentoModel Alterar(int id, [FromBody]FuncionamentoModel funcionamentoModel)
        {
            var funcionamento = funcionamentoModel.ToEntity();
            funcionamento.Id = id;
            funcionamentoServico.Salvar(funcionamento);
            return new FuncionamentoModel().FromEntity(funcionamento);
        }
        [HttpGet, Route("{id}")]
        public FuncionamentoModel GetId(int id)
        {
            var funcionamento = funcionamentoServico.BuscarPorId(id);
            var funcionamentoModel = new FuncionamentoModel().FromEntity(funcionamento);
            return funcionamentoModel;
        }

        [HttpPost, Route()]
        public HttpResponseMessage Pesquisar(FiltrosPesquisaModel filtros)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new FuncionamentoModel[] { });
        }
    }
}