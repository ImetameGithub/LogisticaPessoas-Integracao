import { BaseEntity } from "./BaseEntity";
import { Documento } from "./Documento";

export class DocumentoXProtheus extends BaseEntity {
    IdProtheus: string;
    DescricaoProtheus: string;
    Documento: Documento;
}
