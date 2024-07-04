const baseUrl = 'https://reuniaodiariaapi.imetame.com.br:443/api';
export const environment = {
    production: true,
    urlApi: 'https://reuniaodiariaapi.imetame.com.br:443/api',
    hmr: false,
       //#region PEDIDO
       Pedido: {
        Get: `${baseUrl}/Pedido/Get`,   
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