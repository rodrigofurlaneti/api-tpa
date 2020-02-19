using System;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class AgendamentoViewModel
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public bool Disponivel { get; set; }
        
        public AgendamentoViewModel()
        {
            Data = DateTime.Now;
        }

        public AgendamentoViewModel(Agendamento entidade)
        {
            Id = entidade?.Id ?? 0;
            Data = entidade.Data;
        }

        public Agendamento ToEntity() => new Agendamento()
        {
            Id = Id,
            Data = Data
        };
    }
}