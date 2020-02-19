﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Entidade
{
    public class Clinica : Empresa
    {
        public Clinica()
        {
            DataInsercao = DateTime.Now;
        }

        public virtual bool PossuiAcessibilidadePCD { get; set; }

        public virtual IList<Imagem> GaleriaFotos { get; set; }

        public virtual IList<Arquivo> Arquivos { get; set; }

        public virtual ContaFinanceira ContaFinanceira { get; set; }
        
        public virtual string Responsaveis { get; set; }
        //TODO: Criar HorarioFuncionamento

        
    }
}