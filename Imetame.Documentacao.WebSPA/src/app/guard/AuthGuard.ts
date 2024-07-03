import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import * as _ from 'lodash';


import {
    CanActivate, Router,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
    CanActivateChild,
    NavigationExtras,
    CanLoad, Route
} from '@angular/router';


@Injectable({
    providedIn: 'root', 
})
export class AuthGuard implements CanActivate, CanActivateChild, CanLoad {
    constructor(private oauthService: OAuthService,private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        let url: string = state.url;

        let logado = this.checkLogin(url);
        if (!logado) return logado;

        let temPermissao = this.checkPermission(route.data.claim);
        if (!temPermissao)
            this.router.navigate(['/negado']);

        return temPermissao;
    }

    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        return this.canActivate(route, state);
    }

    canLoad(route: Route): boolean {
        if (route.data.externo && this.checkusuarioExterno()) {
            return true;
        }


        let url =`/${route.path}`;

        let logado = this.checkLogin(url);
        if (!logado) return logado;

        let temPermissao = this.checkPermission(route.data.claim);
        if (!temPermissao)
            this.router.navigate(['/negado']);

        return temPermissao;
    }

    checkLogin(url: string): boolean {
        if (!this.oauthService.hasValidIdToken()) {
            sessionStorage.setItem("redirectTo", url);

            //this.oauthService.initLoginFlow(encodeURIComponent(url));
            //this.oauthService.initLoginFlow();
            
            return false;
        }
        return true;
    }
    checkusuarioExterno(): boolean {
        let user = this.oauthService.getIdentityClaims();
        return !user['interno'];
    }
    checkPermission(claim : any): boolean {
        if (claim) {
            let user = this.oauthService.getIdentityClaims();
            if (user[claim.claimType]) {
                if (_.isArray(user[claim.claimType])) {                    
                    return _.includes(user[claim.claimType], claim.claimValue);

                }
                return user[claim.claimType] == claim.claimValue;

            }
            return false
        }
        return true;
    }
}


/*
Copyright Google LLC. All Rights Reserved.
Use of this source code is governed by an MIT-style license that
can be found in the LICENSE file at http://angular.io/license
*/