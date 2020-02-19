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
    [Authorize, RoutePrefix("api/contafinanceira")]
    public class ContaFinanceiraController : ApiController
    {
        private readonly IContaFinanceiraServico contafinanceiraServico;

        public ContaFinanceiraController(IContaFinanceiraServico contafinanceiraServico)
        {
            this.contafinanceiraServico = contafinanceiraServico;
        }

        [HttpPost, AllowAnonymous, Route("cadastrar")]
        [SwaggerResponse(HttpStatusCode.Created, "Conta financeira cadastrada", typeof(ContaFinanceiraModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Dados inválidos")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno")]
        public ContaFinanceiraModel Cadastrar(ContaFinanceiraModel contafinanceiraModel)
        {
            contafinanceiraModel.Validar();

            var contafinanceira = contafinanceiraModel.ToEntity();
            contafinanceiraServico.Salvar(contafinanceira);

            contafinanceira = contafinanceiraServico.BuscarPorId(contafinanceira.Id);

            var body = $"Olá, foi adicionado a conta financeira \"{contafinanceira.Conta}\". <br />";
            body += $"Conta financeira: {contafinanceira.Conta}<br />";

            return new ContaFinanceiraModel().FromEntity(contafinanceira);
        }

        [HttpPut, Route("{id}/alterar")]
        public ContaFinanceiraModel Alterar(int id, [FromBody]ContaFinanceiraModel contafinanceiraModel)
        {
            var contafinanceira = contafinanceiraModel.ToEntity();
            contafinanceira.Id = id;
            contafinanceiraServico.Salvar(contafinanceira);

            return new ContaFinanceiraModel().FromEntity(contafinanceira);
        }

        [HttpGet, Route("{id}")]
        public ContaFinanceiraModel GetId(int id)
        {
            var contafinanceira = contafinanceiraServico.BuscarPorId(id);
            var contafinanceiraModel = new ContaFinanceiraModel().FromEntity(contafinanceira);
            return contafinanceiraModel;
        }

        [HttpPost, Route()]
        public HttpResponseMessage Pesquisar(FiltrosPesquisaModel filtros)
        {


            return Request.CreateResponse(HttpStatusCode.OK, new ContaFinanceiraModel[] { });
        }
    }
}