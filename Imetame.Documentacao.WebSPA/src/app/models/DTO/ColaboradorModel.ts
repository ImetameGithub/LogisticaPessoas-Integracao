import { BaseEntity } from "../BaseEntity";
import { ColaboradorxAtividade } from "../ColaboradorxAtividade";


export class ColaboradorModel extends BaseEntity {
    Empresa: string;
    NumCad: string;
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
    CountDocumento: number;

    ColaboradorxAtividade?: ColaboradorxAtividade[];
}

export class ColaboradorProtheusModel extends BaseEntity {
    MATRICULA: string;
    CRACHA: string;
    NOME: string;
    NASCIMENTO: string;
    CPF: string;
    RG: string;
    DATA_ADIMISSAO: string;
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