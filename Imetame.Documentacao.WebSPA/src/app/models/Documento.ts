import { BaseEntity } from "./BaseEntity";
import { DocumentoXProtheus } from "./DocumentoXProtheus";

export class Documento extends BaseEntity {
    Descricao: string;
    IdDestra: string;
    DescricaoDestra: string;
    Obrigatorio: boolean = false;

    DocumentoXProtheus: DocumentoXProtheus[] = [];

    /**
     *
     */
    constructor() {
        super();
        this.DocumentoXProtheus = [];
    }
}
