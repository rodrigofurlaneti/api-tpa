using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Agendamento : BaseEntity
    {
        public Agendamento()
        {
            Data = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        [Required(ErrorMessage = "*")]
        public virtual DateTime Data { get; set; }

        public virtual int CapacidadeDeAtendimento { get; set; }
    }
}