import { BaseEntity } from "../BaseEntity";


export class ColaboradorModel extends BaseEntity{
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
}