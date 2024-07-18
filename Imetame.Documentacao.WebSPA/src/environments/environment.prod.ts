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
    //#region PEDIDO
    Documento: {
        Get: `${baseUrl}/Documento/Get`,
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
        GetDocumentosProtheus: `${baseUrl}/Colaboradores/GetDocumentosProtheus`,
        RelacionarColaboradorxAtividade: `${baseUrl}/Colaboradores/RelacionarColaboradorxAtividade`,
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