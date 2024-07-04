
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

    //#region COLABORADORES
    Colaboradores: {
        GetColaboradores: `${baseUrl}/Colaboradores/GetColaboradores`, 
        GetDocumentosProtheus: `${baseUrl}/Colaboradores/GetDocumentosProtheus`, 
    },
    //#endregion

    //#region PROTHEUS
    Protheus: {
        GetOs: `${baseUrl}/Protheus/GetOs`,
    },
    //#endregion
};