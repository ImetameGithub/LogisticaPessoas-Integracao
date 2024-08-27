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