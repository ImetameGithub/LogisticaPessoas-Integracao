const baseUrl = 'https://localhost:7030/api';
export const environment = {
    production: true,
    urlApi: 'https://jaguare.imetame.com.br:5012/api',
    hmr: false,
    // URL PARA API - MATHEUS MONFREIDES - FARTEC SISTEMAS
    //#region PEDIDO
    Pedido: {
        GetItem: `${baseUrl}/Pedido/GetItem`,
        Add: `${baseUrl}/Pedido/Add`,
        Update: `${baseUrl}/Pedido/Update`,
        Delete: `${baseUrl}/Pedido/Delete`,
        GetAll: `${baseUrl}/Pedido/GetAll`,
        GetAllPaginated: `${baseUrl}/Pedido/GetAllPaginated`,
    },
    //#endregion
    //#region CREDENCIADORA
    Credenciadora: {
        GetItem: `${baseUrl}/Credenciadora/GetItem`,
        Add: `${baseUrl}/Credenciadora/Add`,
        Update: `${baseUrl}/Credenciadora/Update`,
        Delete: `${baseUrl}/Credenciadora/Delete`,
        GetAll: `${baseUrl}/Credenciadora/GetAll`,
        GetAllPaginated: `${baseUrl}/Credenciadora/GetAllPaginated`,
    },
    //#endregion

    //#region PEDIDO
    Documento: {
        GetItem: `${baseUrl}/Documento/GetItem`,
        Add: `${baseUrl}/Documento/Add`,
        Update: `${baseUrl}/Documento/Update`,
        Delete: `${baseUrl}/Documento/Delete`,
        GetAll: `${baseUrl}/Documento/GetAll`,
        GetAllPaginated: `${baseUrl}/Documento/GetAllPaginated`,
        GetDocumentosDestra: `${baseUrl}/Documento/GetDocumentosDestra`,
        GetDocumentosProtheus: `${baseUrl}/Documento/GetDocumentosProtheus`,
    },
    //#endregion

    //#region COLABORADORES
    Colaboradores: {
        GetColaboradoresPorOs: `${baseUrl}/Colaboradores/GetColaboradoresPorOs`,
        GetColaboradores: `${baseUrl}/Colaboradores/GetColaboradores`,
        EnviarColaboradorDestra: `${baseUrl}/Colaboradores/EnviarColaboradorDestra`,
        EnviarDocsArrayDestra: `${baseUrl}/Colaboradores/EnviarDocsArrayDestra`,
        EnviarDocumentoParaDestra: `${baseUrl}/Colaboradores/EnviarDocumentoParaDestra`,
        GetDocumentosProtheus: `${baseUrl}/Colaboradores/GetDocumentosProtheus`,
        GetDocumentosObrigatorios: `${baseUrl}/Colaboradores/GetDocumentosObrigatorios`,
        RelacionarColaboradorxAtividade: `${baseUrl}/Colaboradores/RelacionarColaboradorxAtividade`,
        GetImagemProtheus: `${baseUrl}/Colaboradores/GetImagemProtheus`,
        GetItem: `${baseUrl}/Colaboradores/GetItem`,
        Add: `${baseUrl}/Colaboradores/Add`,
        Update: `${baseUrl}/Colaboradores/Update`,
        Delete: `${baseUrl}/Colaboradores/Delete`,
        GetAll: `${baseUrl}/Colaboradores/GetAll`,
        GetAllPaginated: `${baseUrl}/Colaboradores/GetAllPaginated`,
    },
    //#endregion

    //#region ATIVIDADE-ESPECIFICA
    AtividadeEspecifica: {
        GetItem: `${baseUrl}/AtividadeEspecifica/GetItem`,
        Add: `${baseUrl}/AtividadeEspecifica/Add`,
        Update: `${baseUrl}/AtividadeEspecifica/Update`,
        GetAtividadesDestra: `${baseUrl}/AtividadeEspecifica/GetAtividadesDestra`,
        Delete: `${baseUrl}/AtividadeEspecifica/Delete`,
        GetAll: `${baseUrl}/AtividadeEspecifica/GetAll`,
        GetAllPaginated: `${baseUrl}/AtividadeEspecifica/GetAllPaginated`,
    },
    //#endregion

    //#region PROCESSAMENTO
    Processamento: {
        Criar: `${baseUrl}/ProcessamentoFartec/Criar`,
    },
    //#endregion

    //#region PROTHEUS
    Protheus: {
        GetOs: `${baseUrl}/Protheus/GetOs`,
    },
    //#endregion

    //#region DESTRA
    Destra: {
        GetDocumentosDestra: `${baseUrl}/Destra/GetDocumentos`,
    },
    //#endregion
};