import { BaseEntity } from "./BaseEntity";

export class Documento extends BaseEntity {
    Descricao: string;
    IdDestra: string;
    DescricaoDestra: string;
    IdProtheus: string;
    DescricaoProtheus: string;
    Obrigatorio: boolean = false;
}
