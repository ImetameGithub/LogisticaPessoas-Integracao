import { BaseEntity } from "./BaseEntity";
import { Credenciadora } from "./Crendenciadora";

export class Documento extends BaseEntity {
    Descricao: string;
    IdDocCredenciadora: string;
    DescricaoCredenciadora: string;
    IdProtheus: string;
    DescricaoProtheus: string;
    Obrigatorio: boolean = false;

    IdCredenciadora: string;
    Credenciadora: Credenciadora;
}
