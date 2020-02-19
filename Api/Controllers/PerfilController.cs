using Api.Base;
using Api.Models;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    [RoutePrefix("api/perfil")]
    public class PerfilController : ApiController
    {
        private readonly IPerfilServico perfilServico;

        public PerfilController(
            IPerfilServico perfilServico
        )
        {
            this.perfilServico = perfilServico;
        }

        [HttpGet, Route("listar")]
        public IEnumerable<PerfilModel> Listar()
        {
            return perfilServico.Buscar().Select(x => new PerfilModel().FromEntity(x)).ToList();
        }
    }
}
