export class ChecklistModel {
    Nome: string;
    //Matricula: string;
    //Equipe: string;
    //DataAdmissao: string;
    //OrdemServico: string;
    //NumPedido: string;
    Documentos: CheckDocumento[];
    //Cpf: string;
    //StatusDestra: number;
    //Rg: string;
    Atividade: string;
}

export class CheckDocumento {
    IdDestra?: number;
    nome: string;
    impeditivo: string;
    validade: string;
    Status: number;
}