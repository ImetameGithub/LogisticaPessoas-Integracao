import { BaseEntity } from "./BaseEntity";
import { Colaborador } from "./Colaborador";

export class DocumentoxColaborador extends BaseEntity {
    DXC_CODPROTHEUS: string;
    DXC_DESCPROTHEUS: string;
    DXC_CODDESTRA: string;
    DXC_DESCDESTRA: string;
    DXC_BASE64: string;
    DXC_DTENVIO: string;

    DXC_IDCOLABORADOR: string;
    Colaborador: Colaborador;
}
