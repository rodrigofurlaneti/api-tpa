using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Api.Models;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/agendamento")]
    public class AgendamentoController : ApiController
    {
        private readonly IAgendamentoServico _agendamentoServico;
        
        public AgendamentoController(IAgendamentoServico agendamentoServico)
        {
            _agendamentoServico = agendamentoServico;
        }

        [HttpGet]
        [Route("agendamentos")]
        public virtual IEnumerable<AgendamentoModelView> Get()
        {
            return _agendamentoServico.BuscarAgendamentosDisponiveis().Select(x => new AgendamentoModelView(x)).ToList();
        }
        
        [HttpGet]
        [Route("todosAgendamentosPorDia/{dia}/{mes}/{ano}")]
        public virtual IEnumerable<AgendamentoModelView> TodosAgendamentosPorDia(int dia, int mes, int ano)
        {
            return _agendamentoServico.BuscarAgendamentosDisponiveis(dia, mes, ano).Select(x => new AgendamentoModelView(x)).ToList();
        }

        [HttpPost]
        [Route("excluirAgendamento/{id}")]
        public virtual void ExcluirAgendamento(int id)
        {
            _agendamentoServico.ExcluirPorId(id);
        }

        [HttpPost]
        [Route("salvarAgendamentos")]
        public virtual void SalvarAgendamentos(
            //[FromBody] Loja loja
            )
        {
            //if (loja != null)
            //{
            //    var obj = loja.HorarioAgedamentoLoja;
                
            //    foreach (var horario in obj.Where(x => !string.IsNullOrEmpty(x.HoraInicio)))
            //    {
            //        var dia = horario.DataInicio.Day;
            //        var mes = horario.DataInicio.Month;
            //        var ultimomes = horario.AplicarNoAno ? 12 : mes;

            //        while (mes <= ultimomes)
            //        {
            //            if (mes != horario.DataInicio.Month)
            //                dia = 1;

            //            while (dia <= DateTime.DaysInMonth(horario.DataInicio.Year, mes))
            //            {
            //                var hora = int.Parse(horario.HoraInicio.Split(':')[0]);
            //                var minuto = int.Parse(horario.HoraInicio.Split(':')[1]);

            //                var data = new DateTime(horario.DataInicio.Year, mes, dia, hora, minuto, 0);

            //                if ((int)data.DayOfWeek == (int)horario.DiaDaSemana)
            //                {
            //                    var agendamentos = _agendamentoServico.BuscarPor(x => x.Data.Day == dia &&
            //                                                                            x.Data.Month == mes &&
            //                                                                            x.Data.Year == horario.DataInicio.Year);

            //                    foreach (var agendamento in agendamentos)
            //                        _agendamentoServico.Excluir(agendamento);

            //                    if (loja.HorarioFuncionamento == null)
            //                    {
            //                        loja.HorarioFuncionamento = _lojaServico.BuscarPorId(loja.Id).HorarioFuncionamento;

            //                        if (loja.HorarioFuncionamento == null || loja.HorarioFuncionamento.Count == 0)
            //                            throw new Exception("loja não possuí horário de funcionamento cadastro!");
            //                    }

            //                    foreach (var horarioLoja in loja.HorarioFuncionamento.Where(x => (int)x.DiaDaSemana == (int)data.DayOfWeek))
            //                    {
            //                        var horaFim = int.Parse(horarioLoja.HoraFim.Split(':')[0]) == 0 ? 24 : int.Parse(horarioLoja.HoraFim.Split(':')[0]);
            //                        var horaFechamento = Math.Abs(horaFim - int.Parse(horario.Intervalo.Split(':')[0]));
            //                        var minutoFechamento = Math.Abs(int.Parse(horarioLoja.HoraFim.Split(':')[1]) - int.Parse(horario.Intervalo.Split(':')[1]));
            //                        var horaFracionada = 0;

            //                        while (hora <= horaFechamento)
            //                        {
            //                            if ((hora < horaFechamento) || (hora == horaFechamento && minuto <= minutoFechamento))
            //                            {
            //                                data = new DateTime(horario.DataInicio.Year, mes, dia, hora, minuto, 0);

            //                                var agendamento = new Agendamento
            //                                {
            //                                    Data = data,
            //                                    CapacidadeDeAtendimento = horario.CapacidadeAtendimento
            //                                };

            //                                _agendamentoServico.Salvar(agendamento);
            //                            }

            //                            hora += int.Parse(horario.Intervalo.Split(':')[0]);
            //                            minuto = int.Parse(horario.Intervalo.Split(':')[1]);

            //                            if (int.Parse(horario.Intervalo.Split(':')[0]) <= 0)
            //                            {
            //                                horaFracionada += int.Parse(horario.Intervalo.Split(':')[1]);

            //                                if (horaFracionada >= 60)
            //                                    hora++;
            //                            }
            //                        }
            //                    }
            //                }
            //                dia++;
            //            }
            //            mes++;
            //        }
            //    }
            //}
        }
    }
}