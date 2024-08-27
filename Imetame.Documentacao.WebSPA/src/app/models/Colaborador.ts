import { BaseEntity } from "./BaseEntity";
import { ColaboradorxAtividade } from "./ColaboradorxAtividade";
import { DocumentoxColaboradorModel } from "./DTO/DocumentoxColaboradorModel";
import { DocumentoxColaborador } from "./DocumentoxColaborador";

export class Colaborador extends BaseEntity {
    Cracha: string;
    Matricula: string;
    Nome: string;
    MudaFuncao: string;
    Codigo_Funcao: string;
    Nome_Funcao: string;
    SincronizadoDestra: boolean;
    Codigo_Equipe: string;
    Nome_Equipe: string;
    Codigo_Disciplina: string;
    Nome_Disciplina: string;
    Perfil: string;
    Codigo_OS: string;
    Nome_OS: string;
    StatusDestra: number = -2;
    IsAssociado: boolean = false;

    ColaboradorxAtividade: ColaboradorxAtividade[];
    DocumentosxColaborador: DocumentoxColaborador[];
}
