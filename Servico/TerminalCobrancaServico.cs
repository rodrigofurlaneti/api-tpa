﻿using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public interface ITerminalCobrancaServico : IBaseServico<TerminalCobranca>
    {

    }

    public class TerminalCobrancaServico : BaseServico<TerminalCobranca, ITerminalCobrancaRepositorio>, ITerminalCobrancaServico
    {

    }
}
