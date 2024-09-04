export class ColaboradorStatusDestra {
    static NaoSincronizado = new ColaboradorStatusDestra(-2, 'Não Sincronizado'); // CRIADO PARA QUANDO NÃO TEM STATUS DESTRA INFORMADO
    static Recebido = new ColaboradorStatusDestra(0, 'Recebido');
    static Processando = new ColaboradorStatusDestra(1, 'Processando');
    static Sucesso = new ColaboradorStatusDestra(10, 'Sucesso');
    static FalhaProcessamento = new ColaboradorStatusDestra(-1, 'Falha de Processamento');
    static Cancelado = new ColaboradorStatusDestra(-190, 'Cancelado');

    static values = [
        ColaboradorStatusDestra.NaoSincronizado,
        ColaboradorStatusDestra.Recebido,
        ColaboradorStatusDestra.Processando,
        ColaboradorStatusDestra.Sucesso,
        ColaboradorStatusDestra.FalhaProcessamento,
        ColaboradorStatusDestra.Cancelado,
    ];

    public readonly valor: number;
    public readonly nome: string;

    public constructor(valor: number, nome: string) {
        this.valor = valor;
        this.nome = nome;
    }
    static getNameEnum(valor: number): string {
        return ColaboradorStatusDestra.values.find(val => val.valor == valor).nome;
    }
}


export class DocumentoStatusDestra {
    static NaoSincronizado = new DocumentoStatusDestra(-2, 'Não Sincronizado'); // CRIADO PARA QUANDO NÃO TEM STATUS DESTRA INFORMADO
    static Recebido = new DocumentoStatusDestra(0, 'Recebido, Aguardando Processamento');
    static Processando = new DocumentoStatusDestra(1, 'Processando');
    static Sucesso = new DocumentoStatusDestra(10, 'Sucesso');
    static Cancelado = new DocumentoStatusDestra(-10, 'Cancelado');
    static Falha = new DocumentoStatusDestra(-1, 'Falha');

    static values = [
        DocumentoStatusDestra.NaoSincronizado,
        DocumentoStatusDestra.Recebido,
        DocumentoStatusDestra.Processando,
        DocumentoStatusDestra.Sucesso,
        DocumentoStatusDestra.Cancelado,
        DocumentoStatusDestra.Falha,
    ];

    public readonly valor: number;
    public readonly nome: string;

    public constructor(valor: number, nome: string) {
        this.valor = valor;
        this.nome = nome;
    }
    static getNameEnum(valor: number): string {
        return DocumentoStatusDestra.values.find(val => val.valor == valor).nome;
    }
}