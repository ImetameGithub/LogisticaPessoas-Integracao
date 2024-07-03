import { Component, OnInit, OnDestroy } from '@angular/core';
import { FuseConfigService } from '@fuse/services/config.service';
import { OAuthService } from 'angular-oauth2-oidc';
import { Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { FuseSplashScreenService } from '@fuse/services/splash-screen.service';

@Component({
  selector: 'app-signin-oidc',
    template: ''
})
export class SigninOidcComponent implements OnInit, OnDestroy {
  

    private _unsubscribeAll: Subject<any>;


    constructor(private _fuseConfigService: FuseConfigService, private oauthService: OAuthService, private router: Router, private _fuseSplashScreenService: FuseSplashScreenService) {
        this._fuseConfigService.config = {
            layout: {
                navbar: {
                    hidden: true
                },
                toolbar: {
                    hidden: true
                },
                footer: {
                    hidden: true
                },
                sidepanel: {
                    hidden: true
                }
            }
        };

        this._unsubscribeAll = new Subject();
    }

    ngOnInit() {
        
        //this.oauthService.events
        //    .pipe(
        //        takeUntil(this._unsubscribeAll),
        //        filter(e => e.type === 'token_received'))
        //    .subscribe(_ => {
        //        //this.router.navigateByUrl('/perfil');
        //        setTimeout(() => {
        //            this.router.navigateByUrl('/perfil');
        //        }, 500);
        //    });
        this._fuseSplashScreenService.show();
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
        this._fuseSplashScreenService.hide();
    } 


}
