import { Acesso } from './acesso.model';

export class Application {

    constructor() {
        this.permissions = [];
        this.redirectUris = [];
        this.postLogoutRedirectUris = [];
        this.acessos = [];
    }

    id: string;
    clientId: string;    
    uri: string;    
    clientSecret: string;
    displayName: string;
    imagem: string;
    imagemUrl: string;
    permissions: any[];
    redirectUris: any[];
    postLogoutRedirectUris: any[];
    acessos: Acesso[];
}
