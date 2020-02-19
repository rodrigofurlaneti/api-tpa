using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Documento : BaseEntity
    {
        [Required]
        public virtual TipoDocumento Tipo { get; set; }

        [Required]
        public virtual string Numero { get; set; }

        public virtual string OrgaoExpedidor { get; set; }

        public virtual DateTime? DataExpedicao { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        public virtual byte[] ArquivoUpload { get; set; }
    }
}