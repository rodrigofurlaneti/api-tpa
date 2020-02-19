using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class AgendamentoModelView
    {
        public AgendamentoModelView(Agendamento agendamento)
        {
            if (agendamento == null)
                throw new Exception("O agendamento não foi encontrado.");

            Data = agendamento.Data;
            Id = agendamento.Id;
        }

        public int Id { get; set; }

        public DateTime Data { get; set; }
    }
}