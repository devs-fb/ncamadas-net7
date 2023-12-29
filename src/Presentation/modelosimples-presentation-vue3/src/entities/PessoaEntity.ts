export interface Pessoa {
    id: string;
    tipo: string | null;
    pessoaFisica: PessoaFisica | null;
    pessoaJuridica: PessoaJuridica | null;
}

export interface PessoaFisica {
    nomeSocial: string | null;
    dataNascimento: string | null;
    genero: string | null;
}

export interface PessoaJuridica {
    razaoSocial: string | null;
    nomeFantasia: string | null;
    cnae: string | null;
}

export interface PessoaQuery {
    paginacaoOrdenacao: DataInfo;
    pessoa: Pessoa;
}

export interface DataInfo {
    paginacao: PaginacaoInfo;
    ordenacao: OrdenacaoInfo[] | null;
}

export interface PaginacaoInfo {
    pagina: number;
    tamanhoPagina: number;
    totalRegistro: number;
}

export interface OrdenacaoInfo {
    campo: string | null;
    ascendente: boolean;
}
