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
    [Authorize, RoutePrefix("api/preco")]
    public class PrecoController : ApiController
    {
        private readonly IPrecoServico precoServico;
        
        public PrecoController(IPrecoServico precoServico)
        {
            this.precoServico = precoServico;
        }

        [HttpPost, AllowAnonymous, Route("cadastrar")]
        [SwaggerResponse(HttpStatusCode.Created, "Preço cadastrado", typeof(PrecoModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Dados inválidos")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno")]
        public PrecoModel Cadastrar(PrecoModel precoModel)
        {
            precoModel.Validar();

            var preco = precoModel.ToEntity();
            precoServico.Salvar(preco);

            preco = precoServico.BuscarPorId(preco.Id);

            var body = $"Olá, foi adicionado o preço \"{preco.Valor}\". <br />";
            body += $"Valor do preço: {preco.Valor}<br />";

            return new PrecoModel().FromEntity(preco);
        }

        [HttpPut, Route("{id}/alterar")]
        public PrecoModel Alterar(int id, [FromBody]PrecoModel precoModel)
        {
            var preco = precoModel.ToEntity();
            preco.Id = id;
            precoServico.Salvar(preco);

            return new PrecoModel().FromEntity(preco);
        }

        [HttpGet, Route("{id}")]
        public PrecoModel GetId(int id)
        {
            var preco = precoServico.BuscarPorId(id);
            var precoModel = new PrecoModel().FromEntity(preco);
            return precoModel;
        }

        
        [HttpPost, Route()]
        public HttpResponseMessage Pesquisar(FiltrosPesquisaModel filtros)
        {

            return Request.CreateResponse(HttpStatusCode.OK, new PrecoModel[] { });
        }
    }
}
