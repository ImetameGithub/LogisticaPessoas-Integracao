import { Credenciadora } from "app/models/Crendenciadora";

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
    //#region CREDENCIADORA
    Credenciadora: {
        Get: `${baseUrl}/Credenciadora/Get`,   
        Add: `${baseUrl}/Credenciadora/Add`,
        Update: `${baseUrl}/Credenciadora/Update`,
        Delete: `${baseUrl}/Credenciadora/Delete`,
        GetAll: `${baseUrl}/Credenciadora/GetAll`,
        GetAllPaginated: `${baseUrl}/Credenciadora/GetAllPaginated`,
    },
    //#endregion

    //#region COLABORADORES
    Colaboradores: {
        GetColaboradores: `${baseUrl}/Colaboradores/GetColaboradores`,
        GetDocumentosProtheus: `${baseUrl}/Colaboradores/GetDocumentosProtheus`,
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