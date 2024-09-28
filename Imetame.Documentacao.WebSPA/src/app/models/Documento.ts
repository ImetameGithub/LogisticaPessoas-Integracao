import { String, forEach } from "lodash";
import { BaseEntity } from "./BaseEntity";
import { DocumentoXProtheus } from "./DocumentoXProtheus";

export class Documento extends BaseEntity {
    Descricao: string;
    IdDestra: string;
    DescricaoDestra: string;
    Obrigatorio: boolean = false;

    DocumentoXProtheus: DocumentoXProtheus[] = [];


    DescricaoProtheus?: string;
    public GetDescricaoProtheus(): string {
        if (this.DocumentoXProtheus != null) {
            this.DocumentoXProtheus.forEach(x => {
                if (this.DescricaoProtheus == null || this.DescricaoProtheus == undefined)
                    this.DescricaoProtheus = x.DescricaoProtheus;
                else
                    this.DescricaoProtheus = this.DescricaoProtheus + ", " + x.DescricaoProtheus;
            })
        }
        return;
    }

    // Construtor que aceita um objeto do tipo Documento e inicializa a instância
    constructor(init?: Partial<Documento>) {
        super(); // Chamada para o construtor da classe base (BaseEntity)
        // Inicializa as propriedades do objeto com base no parâmetro recebido
        Object.assign(this, init);

        // Chama o método GetDescricaoProtheus após inicialização
        this.GetDescricaoProtheus();
    }
    // constructor() {
    //     super();
    //     this.DocumentoXProtheus = [];
    //     this.GetDescricaoProtheus();
    // }
}
