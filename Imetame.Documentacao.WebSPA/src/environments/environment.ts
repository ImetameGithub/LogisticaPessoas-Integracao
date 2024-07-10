const baseUrl = 'https://localhost:7030/api';
export const environment = {
    production: true,
    urlApi: 'https://localhost:7030/api',
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

    //#region COLABORADORES
    Colaboradores: {
        GetColaboradoresPorOs: `${baseUrl}/Colaboradores/GetColaboradoresPorOs`,
        GetColaboradores: `${baseUrl}/Colaboradores/GetColaboradores`,
        GetDocumentosProtheus: `${baseUrl}/Colaboradores/GetDocumentosProtheus`,
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
        Delete: `${baseUrl}/AtividadeEspecifica/Delete`,
        GetAll: `${baseUrl}/AtividadeEspecifica/GetAll`,
        GetAllPaginated: `${baseUrl}/AtividadeEspecifica/GetAllPaginated`,
    },
    //#endregion

    //#region PROTHEUS
    Protheus: {
        GetOs: `${baseUrl}/Protheus/GetOs`,
    },
    //#endregion
};