import { BaseEntity } from "../BaseEntity";


export class ColaboradorModel extends BaseEntity {
    Empresa: string;
    numCad: string;
    NumCracha: string;
    Status: string;
    Nome: string;
    Cpf: string;
    FuncaoAtual: string;
    FuncaoInicial: string;
    DataAdmissao: Date;
    DataNascimento: Date;
    Equipe: string;
    Perfil: string;
    Endereco: string;
    Numero: string;
    Bairro: string;
    Cidade: string;
    Cep: string;
    Ddd: string;
    Numtel: string;
    Estado: string;
    TempoEmpresaAnos: string;
    TempoEmpresaAnosInt: string;
}

export class ColaboradorProtheusModel extends BaseEntity {
    MATRICULA: string;
    CRACHA: string;
    NOME: string;
    CODIGO_FUNCAO: string;
    NOME_FUNCAO: string;
    CODIGO_EQUIPE: string;
    NOME_EQUIPE: string;
    CODIGO_DISCIPLINA: string;
    NOME_DISCIPLINA: string;
    PERFIL: string;
    CODIGO_OS: string;
    NOME_OS: string
}