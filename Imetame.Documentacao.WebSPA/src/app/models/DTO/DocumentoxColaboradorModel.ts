import { DocumentoStatus } from "./ColaboradorModel";

export class DocumentoxColaboradorModel {
    // Id: string;
    Codigo: string;
    DescArquivo: string;
    DtVencimento: string;
    DtVencimentoFormatada: Date;
    NomeColaborador: string;
    Matricula: string;
    NomeArquivo: string;
    Recno: string;
    Bytes: string;
    Base64: string;
    Vencido: boolean;
    Vencer: boolean;
    DiasVencer: number;
    SincronizadoDestra: boolean;
    RelacionadoDestra: boolean;
    ImageSource;
    TipoDocumento: string;
    IdTipoDocumento: string;
    DocumentoStatus: DocumentoStatus;
    DocumentoStatusPendente: boolean;
}

export class ImagemProtheus {
    Bytes: string;
    Base64: string;
}
