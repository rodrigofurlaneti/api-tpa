using System.Linq;
using System.Web.Http;
using Api.Base;
using Core.Exceptions;
using System.Net;
using Api.Models;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/pessoa")]
    public class PessoaController : BaseController<Pessoa, IPessoaServico>
    {
        private readonly IEnderecoServico _enderecoServico;
        private readonly IDocumentoServico _documentoServico;
        private readonly IContatoServico _contatoServico;
        
        public PessoaController(IEnderecoServico enderecoServico, IDocumentoServico documentoServico, IContatoServico contatoServico)
        {
            _enderecoServico = enderecoServico;
            _documentoServico = documentoServico;
            _contatoServico = contatoServico;
        }

        [HttpGet]
        [Route("{id}")]
        public override Pessoa Get(int id)
        {
            try
            {
                var pessoa = Servico.BuscarPorId(id);

                //if (pessoa?.Cartoes != null && pessoa.Cartoes.Any())
                //    pessoa.Cartoes = _cartaoServico.DescriptografarCartoes(pessoa.Cartoes);

                return pessoa;
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("BuscarPorCPF/{cpf}")]
        public PessoaModel GetByCPF(string cpf)
        {
            try
            {
                var pessoa = Servico.BuscarPor(x => x.Documentos.Count(y => y.Numero.Replace(".", "").Replace("-", "").Equals(cpf)) > 0).LastOrDefault();

                return new PessoaModel(pessoa);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        //[HttpGet]
        //[Route("ObterPorCartao/{numero}")]
        //public Pessoa ObterPorCartao(string numero)
        //{
        //    return Servico.ObterPorCartao(numero);
        //}

        [HttpPost]
        [Route("{id}/adicionaritem")]
        public Pessoa AdicionarItem(int id, Endereco item)
        {
            try
            {
                var pessoa = Servico.BuscarPorId(id);

                pessoa.Enderecos.Add(item);

                return pessoa;
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}