import { BaseEntity } from "./BaseEntity";

export class AtividadeEspecifica extends BaseEntity {
    Codigo: string;
    Descricao: string;
    IdDestra: number;
    QuantColaboradores: number = 0;
}