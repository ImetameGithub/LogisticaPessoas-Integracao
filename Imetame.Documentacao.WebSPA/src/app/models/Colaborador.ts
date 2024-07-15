import { BaseEntity } from "./BaseEntity";
import { ColaboradorxAtividade } from "./ColaboradorxAtividade";

export class Colaborador extends BaseEntity{
    Cracha: string;
    Matricula: string;
    Nome: string;
    MudaFuncao: string;
    Codigo_Funcao: string;
    Nome_Funcao: string;
    Codigo_Equipe: string;
    Nome_Equipe: string;
    Codigo_Disciplina: string;
    Nome_Disciplina: string;
    Perfil: string;
    Codigo_OS: string;
    Nome_OS: string;

    ColaboradorxAtividade: ColaboradorxAtividade[];
}
  