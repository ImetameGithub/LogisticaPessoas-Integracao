export class ColaboradorStatusDestra {
    static NaoSincronizado = new ColaboradorStatusDestra(-2, 'Não Sincronizado'); // CRIADO PARA QUANDO NÃO TEM STATUS DESTRA INFORMADO
    static Recebido = new ColaboradorStatusDestra(0, 'Recebido');
    static Processando = new ColaboradorStatusDestra(1, 'Processando');
    static Sucesso = new ColaboradorStatusDestra(10, 'Sincronizado');
    static FalhaProcessamento = new ColaboradorStatusDestra(-1, 'Desafio nos Documentos');
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
    static NaoSincronizado = new DocumentoStatusDestra(-2, 'Não Sincronizado','FFCCCC00'); // CRIADO PARA QUANDO NÃO TEM STATUS DESTRA INFORMADO
    static Recebido = new DocumentoStatusDestra(0, 'Recebido, Aguardando Processamento','0000FF');
    static Processando = new DocumentoStatusDestra(1, 'Processando','0000FF');
    static Sucesso = new DocumentoStatusDestra(10, 'Sucesso','FF00FF00');
    static Cancelado = new DocumentoStatusDestra(-10, 'Cancelado','FF0000');
    static Falha = new DocumentoStatusDestra(-1, 'Falha','FF0000');

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
    public readonly cor: string;

    public constructor(valor: number, nome: string, cor: string) {
        this.valor = valor;
        this.nome = nome;
        this.cor = cor;
    }
    static getNameEnum(valor: number): string {
        return DocumentoStatusDestra.values.find(val => val.valor == valor).nome;
    }
    static getColorEnum(valor: number): string {
        return DocumentoStatusDestra.values.find(val => val.valor == valor).cor;
    }
}