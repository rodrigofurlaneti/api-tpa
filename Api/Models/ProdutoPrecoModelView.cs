﻿using System;
using Entidade;

namespace Api.Models
{
    public class ProdutoPrecoModelView
    {
        public ProdutoPrecoModelView(Produto produto, ProdutoPreco preco)
        {
            if (produto == null)
                throw new Exception("O produto não foi encontrado.");

            if (preco == null)
                throw new Exception("Nenhum preço foi encontrado para o produto.");

            if (preco.Fornecedor != null)
                Fornecedor = new FornecedorModelView(preco.Fornecedor);

            if (preco.Loja != null)
                Loja = new LojaModelView(preco.Loja);

            Produto = new ProdutoModelView(produto);
            Quantidade = 1;
            ValorDesconto = preco.ValorDesconto;
            Valor = preco.Valor;
            Id = preco.Id;
            InicioVigencia = preco.InicioVigencia.ToShortDateString();
            FimVigencia = preco.FimVigencia.ToShortDateString();
            Status = preco.Status;
        }

        public ProdutoModelView Produto { get; set; }

        public LojaModelView Loja { get; set; }

        public FornecedorModelView Fornecedor { get; set; }

        public int Id { get; set; }

        public int Quantidade { get; set; }

        public decimal Valor { get; set; }

        public decimal ValorDesconto { get; set; }

        public string InicioVigencia { get; set; }

        public string FimVigencia { get; set; }

        public bool Status { get; set; }
    }
}