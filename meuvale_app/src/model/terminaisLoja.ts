export class TerminaisLoja {
    Id: number;
    Loja: { Id: number };
    Usuario: { Id: string };
    Terminal: Terminal;
}

export class Terminal {
        Id: number;
        Maquininha: string;
        Modelo: string;
        NumeroSerial: string;
        NomeGerenciadora: string;
        TaxaRefeicao: number;
        TaxaAlimentacao: number;
        TaxaCombustivel: number;
        TaxaAdiantamento: number;
}