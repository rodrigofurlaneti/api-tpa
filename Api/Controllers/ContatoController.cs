using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Api.Base;
using Api.Models;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    public class ContatoController : BaseController<Contato, IContatoServico>
    {
		[HttpPost, AllowAnonymous]
		public HttpResponseMessage Enviar(ContatoModel model)
		{

			return Request.CreateResponse(System.Net.HttpStatusCode.Created, model);
		}
	}
}