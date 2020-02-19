using System;
using System.Configuration;
using System.Linq;
using System.Web;
using Core.Exceptions;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;

namespace Dominio
{
    public interface IUsuarioServico : IBaseServico<Usuario>
    {
        Usuario ValidarLogin(string usuario, string senha);
        void RecuperarSenha(string login, string template);
        void TrocarSenha(string usuario, string senha, string senhaConfirmacao);
        void PrimeiroLoginRealizado(string usuario);
        Usuario RetornarPorUsuario(string usuario);
        Usuario Register(Pessoa pessoa, string senha, int id, string facebookId = "", int perfilId = 0);
        void EncryptAllPasswords();
        Usuario LoginFacebook(string email, string facebookId);
    }

    public class UsuarioServico : BaseServico<Usuario, IUsuarioRepositorio>, IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public Usuario ValidarLogin(string login, string senha)
        {
            var pass = Crypt.EnCrypt(senha, ConfigurationManager.AppSettings["CryptKey"]);
            return _usuarioRepositorio.ValidarLogin(login, pass, null);
        }

        public void RecuperarSenha(string login, string template)
        {
            var usuarioSolicitado = Repositorio.FirstBy(x => x.Login == login);
            if (string.IsNullOrWhiteSpace(usuarioSolicitado?.Login))
                throw new NotFoundException("Nenhum usuário foi encontrado para o login informado.");
            if (string.IsNullOrEmpty(usuarioSolicitado?.Pessoa?.Email))
                throw new NotFoundException("O usuário não possui um email cadastrado!");

            //para quem vai o email
            var from = ConfigurationManager.AppSettings["EMAIL_FROM"];
            //muda dinâmicamente as variáveis no template do email
            template = template.Replace("[LINK]", GerarLinkRecuperarSenha(usuarioSolicitado.Login));
            //chama a classe que envia o email
            Mail.SendMail(usuarioSolicitado?.Pessoa?.Email, "[St. Marche] Recuperar senha", template, from);
        }
        
        public void TrocarSenha(string usuario, string senha, string senhaConfirmacao)
        {
            if (!senha.Equals(senhaConfirmacao))
                throw new Exception("A senha e a confirmação devem ser iguais.");

            var usuarioSolicitado = Repositorio.FirstBy(x => x.Login.Equals(usuario));
            if (usuarioSolicitado == null)
                throw new Exception("Nenhum usuário foi encontrado para o e-mail informado.");

            var pass = Crypt.EnCrypt(senha, ConfigurationManager.AppSettings["CryptKey"]);
            usuarioSolicitado.Senha = pass;
            usuarioSolicitado.IsEncrypted = true;
            Salvar(usuarioSolicitado);
        }
        
        public Usuario Register(Pessoa pessoa, string senha, int id, string facebookId = "", int perfilId = 0)
        {
            try
            {
                var usuario = Repositorio.GetById(id);

                if (usuario == null)
                    usuario = new Usuario
                    {
                        UltimoAcesso = DateTime.Now,
                        Perfil = perfilId > 0 ? perfilId : (int)PerfilApp.Consumer,
                        FacebookId = facebookId,
                        IsEncrypted = true,
                        Pessoa = pessoa,
                        PrimeiroLogin = false,
                        Ativo = true
                    };

                //usuario.Login = pessoa.Documentos.FirstOrDefault(x => x.Tipo.Equals((int)TipoDocumento.Cpf))?.Numero.Replace(".","").Replace("-","").Replace(".","");
                usuario.Login = pessoa.Email;
                usuario.Senha = string.IsNullOrEmpty(senha) ? string.Empty : Crypt.EnCrypt(senha, ConfigurationManager.AppSettings["CryptKey"]);

                Salvar(usuario);
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public void EncryptAllPasswords()
        {
            using (var transaction = Repositorio.SetupNewTransaction())
            {
                BuscarPor(x => !x.IsEncrypted && x.Pessoa != null).ToList().ForEach(x =>
                {
                    x.Senha = Crypt.EnCrypt(x.Senha, ConfigurationManager.AppSettings["CryptKey"]);
                    x.IsEncrypted = true;
                });

                transaction.Commit();
                transaction.Dispose();
            }
        }

        public Usuario LoginFacebook(string email, string facebookId)
        {
            var usuario = PrimeiroPor(u => u.Pessoa.Contatos.Any(c => c.Contato.Email == email) || u.Login == email);

            if (usuario != null)
            {
                if (string.IsNullOrEmpty(usuario.FacebookId))
                {
                    usuario.FacebookId = facebookId;
                    Salvar(usuario);
                }

                return usuario;
            }

            throw new BusinessRuleException("E-mail inexistente");
        }



        private string GerarLinkRecuperarSenha(string usuario)
        {
            var dataEncrypt = Crypt.EnCrypt(usuario.ToBase64(), ConfigurationManager.AppSettings["CryptKey"]);
            dataEncrypt = HttpUtility.UrlEncode(dataEncrypt);
            var link = $"{ConfigurationManager.AppSettings["HOST"]}Account/RecuperarMinhaSenha?key={dataEncrypt}";
            return link;
        }

        public void PrimeiroLoginRealizado(string user)
        {
            var usuario = RetornarUsuarioPor(user);
            if (usuario == null) throw new Exception("Email não encontrado!");

            usuario.PrimeiroLogin = false;
            Repositorio.Save(usuario);
        }

        private Usuario RetornarUsuarioPor(string usuario = "", string senha = "")
        {
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senha))
                return Repositorio.FirstBy(x => x.Login.Trim() == usuario.Trim() && x.Senha == senha);

            return !string.IsNullOrEmpty(usuario) ? Repositorio.FirstBy(x => x.Login.Trim() == usuario.Trim()) : null;
        }

        public Usuario RetornarPorUsuario(string usuario)
        {
            return _usuarioRepositorio.RetornarPorUsuario(usuario);
        }
    }
}