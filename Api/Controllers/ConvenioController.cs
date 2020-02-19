using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Api.Base;
using Dominio;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace Api.Controllers
{
    [RoutePrefix("api/convenio")]
    public class ConvenioController : BaseController<Convenio, IConvenioServico>
    {
        [HttpGet]
        [Route("PorNomeDocumentoOuCidade")]
        public IEnumerable<Convenio> GetPorNomeDocumentoOuCidade(int estado, int cidade, string bairro, string dadosPesquisa, int inicio, int quantidade)
        {
            if (quantidade < 0)
                quantidade = 0;

            var lojas = Servico.BuscaPor(estado, cidade, bairro, dadosPesquisa).Skip(inicio).Take(quantidade);
            
            return lojas.OrderBy(x => x.Descricao);
        }
        
        [HttpGet]
        [Route("{id}/planos")]
        public IEnumerable<PlanoVenda> GetPlanos(int id, int inicio, int quantidade)
        {
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<PlanoVendaServico>();
            var produtos = new List<PlanoVenda>();
            return produtoPrecoBusiness.BuscarPor(x => x.Convenio.Id == id).Skip(inicio).Take(quantidade);
        }
    }
}