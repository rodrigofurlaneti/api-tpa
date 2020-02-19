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
    [Authorize, RoutePrefix("api/imagem")]
    public class ImagemController : ApiController
    {
        private readonly IImagemServico imagemServico;

        public ImagemController(IImagemServico imagemServico)
        {
            this.imagemServico = imagemServico;
        }

        [HttpPost, AllowAnonymous, Route("cadastrar")]
        [SwaggerResponse(HttpStatusCode.Created, "Imagem cadastrada", typeof(ImagemModel))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Dados inválidos")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Erro interno")]
        public ImagemModel Cadastrar(ImagemModel imagemModel)
        {
            imagemModel.Validar();

            var imagem = imagemModel.ToEntity();
            imagemServico.Salvar(imagem);

            imagem = imagemServico.BuscarPorId(imagem.Id);

            var body = $"Olá, foi adicionado a imagem \"{imagem.ImagemUpload}\". <br />";
            body += $"Nome do Imagem: {imagem.ImagemUpload}<br />";

            return new ImagemModel().FromEntity(imagem);
        }

        [HttpPut, Route("{id}/alterar")]
        public ImagemModel Alterar(int id, [FromBody]ImagemModel imagemModel)
        {
            var imagem = imagemModel.ToEntity();
            imagem.Id = id;
            imagemServico.Salvar(imagem);

            return new ImagemModel().FromEntity(imagem);
        }

        [HttpGet, Route("{id}")]
        public ImagemModel GetId(int id)
        {
            var imagem = imagemServico.BuscarPorId(id);
            var imagemModel = new ImagemModel().FromEntity(imagem);
            return imagemModel;
        }

        [HttpPost, Route()]
        public HttpResponseMessage Pesquisar(FiltrosPesquisaModel filtros)
        {


            return Request.CreateResponse(HttpStatusCode.OK, new ImagemModel[] { });
        }
    }
}