using Entidade.Base;

namespace Entidade
{
    public class UsuarioPerfil
    {
        public virtual Perfil Perfil { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}