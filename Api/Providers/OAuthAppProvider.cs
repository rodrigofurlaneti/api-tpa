using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dominio;
using Entidade;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.ServiceLocation;
using Repositorio.Base;

namespace Api.Providers
{
    public class OAuthAppProvider : OAuthAuthorizationServerProvider
    {
        #region Private Members
        private readonly string _publicClientId;

        private static bool IsPersistent(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var form = context.Request.ReadFormAsync().Result;
            return form["IsPersistent"] != null &&
                   (form["IsPersistent"].Equals("on", StringComparison.InvariantCultureIgnoreCase) ||
                    form["IsPersistent"].Equals("true", StringComparison.InvariantCultureIgnoreCase));
        }
        #endregion

        public OAuthAppProvider(string publicClientId)
        {
            if (string.IsNullOrEmpty(publicClientId))
                throw new ArgumentNullException(nameof(publicClientId));

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var username = context.UserName;
                var password = context.Password;
                var nHibSession = ServiceLocator.Current.GetInstance<NHibSession>();
                var usuarioServ = ServiceLocator.Current.GetInstance<IUsuarioServico>();
                var empresaServ = ServiceLocator.Current.GetInstance<IEmpresaServico>();

                var usuario = usuarioServ.ValidarLogin(username, password);
                if (usuario == null)
                {
                    context.SetError("invalid_grant", "Usuário ou senha incorretos.");
                    return;
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Pessoa != null ? usuario.Pessoa.Nome : usuario.Login),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                };

                // Adiciona aos claims as permissões do usuário
                claims.AddRange(usuario.Perfils.SelectMany(x => x.Perfil.Permissoes).Select(x => new Claim(ClaimTypes.Role, x.Regra)));

                var empresa = empresaServ.PrimeiroPor(x => x.Responsavel.Usuario.Id == usuario.Id);

                var oAuthIdentity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                var cookiesIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);

                empresa = (Empresa)nHibSession.Unproxy(empresa);

                var properties = CreateProperties(usuario, empresa, null);
                properties.IsPersistent = IsPersistent(context);
                var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (Exception ex)
            {
                context.SetError("internal_error", ex.Message);
                throw;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                var expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// This method returns the properties that will be returned in the response token request
        /// </summary>
        public static AuthenticationProperties CreateProperties(Usuario usuario, Empresa empresa, string clientId)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "UsuarioId", usuario.Id.ToString() },
                { "UsuarioNome", usuario.Pessoa != null ? usuario.Pessoa.Nome : usuario.Login },
                { "PerfilId", usuario.Perfil.ToString() },
                { "PessoaId", usuario.Pessoa != null ? usuario.Pessoa.Id.ToString() : usuario.Login },
                { "PrimeiroLogin", usuario.PrimeiroLogin.ToString() }
            };

            if(empresa is Clinica)
            {
                data.Add("ClinicaId", empresa?.Id.ToString());
                data.Add("ClinicaNome", empresa?.Nome);
            }
            else
            {
                data.Add("EmpresaId", empresa?.Id.ToString());
                data.Add("EmpresaNome", empresa?.Nome);
            }

            return new AuthenticationProperties(data);
        }
    }
}