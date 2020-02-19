using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Api.Base;
using Api.Models;
using Core.Exceptions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/fornecedor")]
    public class FornecedorController : BaseController<Fornecedor, IFornecedorServico>
    {
        [HttpGet]
        [Route("fornecedores")]
        public IEnumerable<FornecedorModelView> GetFornecedores()
        {
            var fornecedores = new List<FornecedorModelView>();
            var produtoPrecoBusiness = ServiceLocator.Current.GetInstance<IProdutoPrecoServico>();

            foreach (var fornecedor in Servico.Buscar())
            {
                var fornecedorModelView = new FornecedorModelView(fornecedor);
                    fornecedores.Add(fornecedorModelView);
            }

            return fornecedores;
        }

        [HttpGet]
        [Route("fornecedoresProximos")]
        public IEnumerable<FornecedorModelView> GetFornecedores(float latitude, float longitude, int inicio, int quantidade)
        {
            var lojas = new List<FornecedorModelView>();
            var lojasBase = Servico.Buscar();

            foreach (var loja in lojasBase)
                lojas.Add(new FornecedorModelView(loja, latitude, longitude));

            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia).Skip(inicio).Take(quantidade);
        }

        [HttpGet]
        [Route("fornecedoresProximosPorClassificacao")]
        public IEnumerable<FornecedorModelView> GetFornecedoresPorClassificacao(float latitude, float longitude, int inicio, int quantidade, string classificacao)
        {
            var lojas = new List<FornecedorModelView>();
            var lojasBase = Servico.BuscarPor(x => x.Classificacao == classificacao || x.Classificacao == TipoClassificacao.Ambos.ToString());

            foreach (var loja in lojasBase)
                lojas.Add(new FornecedorModelView(loja, latitude, longitude));

            return lojas.OrderBy(x => x.Distancia <= 0d).ThenBy(y => y.Distancia).Skip(inicio).Take(quantidade);
        }

        [HttpPost]
        [Route("salvarFornecedor/{id}")]
        public virtual void SalvarFornecedor(int id, [FromBody] Fornecedor entity)
        {
            try
            {
                Servico.ValidaESalva(id, entity);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("salvarProdutoPrecoFornecedor/{id}")]
        public virtual void SalvarProdutoPrecoLoja(int id, [FromBody] ProdutoPreco produtoPreco)
        {
            try
            {
                Servico.SalvarProdutoPrecoFornecedor(id, produtoPreco);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}