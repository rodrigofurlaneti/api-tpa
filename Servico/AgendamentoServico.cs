using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IAgendamentoServico : IBaseServico<Agendamento>
    {
        IList<Agendamento> BuscarAgendamentosDisponiveis();
        IList<Agendamento> BuscarAgendamentosDisponiveis(int dia, int mes, int ano);
    }

    public class AgendamentoServico : BaseServico<Agendamento, IAgendamentoRepositorio>, IAgendamentoServico
    {
        public IList<Agendamento> BuscarAgendamentosDisponiveis()
        {
            return BuscarPor(x => x.Data >= DateTime.Now).ToList();
        }

        public IList<Agendamento> BuscarAgendamentosDisponiveis(int dia, int mes, int ano)
        {
            return BuscarPor(x => x.Data.Day == dia &&
                                    x.Data.Month == mes &&
                                    x.Data.Year == ano).ToList();
        }
    }
}