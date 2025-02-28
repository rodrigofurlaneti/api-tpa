﻿using System.Web.Http;
using Api.Base;
using System.Collections.Generic;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/cidade")]
    public class CidadeController : BaseController<Cidade, ICidadeServico>
    {
        [HttpGet]
        [Route("getCidadesByEstados/{id}")]
        public virtual IList<Cidade> GetCidadesByEstados(int id)
        {
            var result = Servico.BuscarPor(x=>x.Estado.Id == id);
            return result;
        }
    }
}