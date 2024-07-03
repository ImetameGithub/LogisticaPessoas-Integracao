import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { DOCUMENT, Location } from '@angular/common';
import { Platform } from '@angular/cdk/platform';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { takeUntil, filter } from 'rxjs/operators';

import { FuseConfigService } from '@fuse/services/config.service';
import { FuseNavigationService } from '@fuse/components/navigation/navigation.service';
import { FuseSidebarService } from '@fuse/components/sidebar/sidebar.service';
import { FuseSplashScreenService } from '@fuse/services/splash-screen.service';
import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';

import { navigation } from 'app/navigation/navigation';

//import { locale as navigationEnglish } from 'app/navigation/i18n/en';
//import { locale as navigationTurkish } from 'app/navigation/i18n/tr';
import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { Router, ActivatedRoute } from '@angular/router';
import { IDENTITY_URL } from './config/tokens';
import { authConfig } from './config/authConfig';
import { FuseNavigationItem, FuseClaim } from '../@fuse/types';
import * as _ from 'lodash';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
    fuseConfig: any;
    navigation: any;
    user: any;
    // Private
    private _unsubscribeAll: Subject<any>;

    /**
     * Constructor
     *
     * @param {DOCUMENT} document
     * @param {FuseConfigService} _fuseConfigService
     * @param {FuseNavigationService} _fuseNavigationService
     * @param {FuseSidebarService} _fuseSidebarService
     * @param {FuseSplashScreenService} _fuseSplashScreenService
     * @param {FuseTranslationLoaderService} _fuseTranslationLoaderService
     * @param {Platform} _platform
     * @param {TranslateService} _translateService
     */
    constructor(
        @Inject(DOCUMENT) private document: any,
        private _fuseConfigService: FuseConfigService,
        private _fuseNavigationService: FuseNavigationService,
        private _fuseSidebarService: FuseSidebarService,
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _fuseTranslationLoaderService: FuseTranslationLoaderService,
        private _translateService: TranslateService,
        private _platform: Platform,
        private oauthService: OAuthService,
        private router: Router,
        private location: Location,
        @Inject(IDENTITY_URL) private identityUrl: string
    ) {

        this._unsubscribeAll = new Subject();







        // Get default navigation
        this.navigation = navigation;

        // Register the navigation to the service
        this._fuseNavigationService.register('main', navigation);



        // Set the main navigation as our current navigation
        this._fuseNavigationService.setCurrentNavigation('main');

        this.configureCodeFlow();

        // Add languages
        //this._translateService.addLangs(['en', 'tr']);

        // Set the default language
        //this._translateService.setDefaultLang('en');

        // Set the navigation translations
        //this._fuseTranslationLoaderService.loadTranslations(navigationEnglish, navigationTurkish);

        // Use a language
        //this._translateService.use('en');

        /**
         * ----------------------------------------------------------------------------------------------------
         * ngxTranslate Fix Start
         * ----------------------------------------------------------------------------------------------------
         */

        /**
         * If you are using a language other than the default one, i.e. Turkish in this case,
         * you may encounter an issue where some of the components are not actually being
         * translated when your app first initialized.
         *
         * This is related to ngxTranslate module and below there is a temporary fix while we
         * are moving the multi language implementation over to the Angular's core language
         * service.
         **/

        // Set the default language to 'en' and then back to 'tr'.
        // '.use' cannot be used here as ngxTranslate won't switch to a language that's already
        // been selected and there is no way to force it, so we overcome the issue by switching
        // the default language back and forth.
        /**
         setTimeout(() => {
            this._translateService.setDefaultLang('en');
            this._translateService.setDefaultLang('tr');
         });
         */

        /**
         * ----------------------------------------------------------------------------------------------------
         * ngxTranslate Fix End
         * ----------------------------------------------------------------------------------------------------
         */

        // Add is-mobile class to the body if the platform is mobile
        if (this._platform.ANDROID || this._platform.IOS) {
            this.document.body.classList.add('is-mobile');
        }
        //this.user = this.oauthService.getIdentityClaims();
        // Set the private defaults

    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {

        // Subscribe to config changes
        this._fuseConfigService.config
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config) => {

                this.fuseConfig = config;

                // Boxed
                if (this.fuseConfig.layout.width === 'boxed') {
                    this.document.body.classList.add('boxed');
                }
                else {
                    this.document.body.classList.remove('boxed');
                }

                // Color theme - Use normal for loop for IE11 compatibility
                for (let i = 0; i < this.document.body.classList.length; i++) {
                    const className = this.document.body.classList[i];

                    if (className.startsWith('theme-')) {
                        this.document.body.classList.remove(className);
                    }
                }

                this.document.body.classList.add(this.fuseConfig.colorTheme);
            });

        //this.configureNavigation();
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Toggle sidebar open
     *
     * @param key
     */
    toggleSidebarOpen(key): void {
        this._fuseSidebarService.getSidebar(key).toggleOpen();
    }


    private configureCodeFlow() {
        this.configureNavigation();

        this.oauthService.configure(authConfig(this.identityUrl));
        // this.oauthService.setStorage(localStorage);
        this.oauthService.tokenValidationHandler = new JwksValidationHandler();


        this.oauthService.loadDiscoveryDocumentAndTryLogin().then(_ => {
            if (!this.oauthService.hasValidIdToken() || !this.oauthService.hasValidAccessToken()) {
                //this.oauthService.initImplicitFlow();
                let path = sessionStorage.getItem("redirectTo") || this.location.path();
                sessionStorage.removeItem("redirectTo");
                this.oauthService.initLoginFlow(path);

                return false;
            } else {

                if (this.router.url.startsWith('/signin-oidc')) {
                    if (this.oauthService.state != null && this.oauthService.state != '/signin-oidc')
                        this.router.navigateByUrl(decodeURIComponent(this.oauthService.state));
                    else
                        this.router.navigateByUrl('/');
                }
                this.configureNavigation();
                return true;
            }
        });/*.then(logou => {
            if (logou) {                    
                setTimeout(() => {
                    if (this.router.url === '/signin-oidc')
                        this.router.navigateByUrl('/');
                    }, 100);
                }
            });*/


        // Optional
        //this.oauthService.setupAutomaticSilentRefresh();

        // Display all events
        //this.oauthService.events.subscribe(e => {
        //    // tslint:disable-next-line:no-console
        //    console.debug('oauth/oidc event', e);
        //});

        //this.oauthService.events
        //    .pipe(filter(e => e.type === 'session_terminated'))
        //    .subscribe(e => {
        //        // tslint:disable-next-line:no-console
        //        console.debug('Your session has been terminated!');
        //    });

        // Automatically load user profile
        this.oauthService.events
            .pipe(filter(e => e.type === 'token_received'))
            .subscribe(_ => {
                setTimeout(() => {
                    if (this.router.url.startsWith('/signin-oidc')) {
                        if (this.oauthService.state != null && this.oauthService.state != '/signin-oidc')
                            this.router.navigateByUrl(decodeURIComponent(this.oauthService.state));
                        else
                            this.router.navigateByUrl('/');
                    }
                }, 100);


                this.oauthService.loadUserProfile();
            });


        this.oauthService.events
            .pipe(takeUntil(this._unsubscribeAll), filter(e => e.type === 'user_profile_loaded'))
            .subscribe(e => {

                this.user = this.oauthService.getIdentityClaims();
                this.configureNavigation();
            });

        this.oauthService.events
            .pipe(filter(e => e.type === 'token_expires'))
            .subscribe(e => {

                this.oauthService.refreshToken()
                    .catch(err => {
                        console.log(err);

                        sessionStorage.removeItem('access_token');
                        sessionStorage.removeItem('id_token');
                        sessionStorage.removeItem('refresh_token');
                        sessionStorage.removeItem('nonce');
                        sessionStorage.removeItem('expires_at');
                        sessionStorage.removeItem('id_token_claims_obj');
                        sessionStorage.removeItem('id_token_expires_at');
                        sessionStorage.removeItem('id_token_stored_at');
                        sessionStorage.removeItem('access_token_stored_at');
                        sessionStorage.removeItem('granted_scopes');
                        sessionStorage.removeItem('session_state');


                        this.oauthService.initLoginFlow();
                        return;
                    });
            });

    }

    private configureNavigation() {

        this.user = this.oauthService.getIdentityClaims();
        var navigation = this._fuseNavigationService.getCurrentNavigation();
        //hidden para todos
        for (const item of navigation) {
            this.hiddenNavItem(item);
        }

        if (_.isUndefined(this.user) || _.isNull(this.user) || !this.oauthService.hasValidAccessToken())
            return;


        for (const item of navigation) {
            this.showNavItem(item);
        }

    }

    private hiddenNavItem(item: FuseNavigationItem) {
        item.hidden = true;
        if (item.children) {
            for (const children of item.children) {
                this.hiddenNavItem(children);
            }
        }
    }

    private showNavItem(item: FuseNavigationItem): boolean {
        let hidden = true;
        if (item.children) {
            for (const children of item.children) {
                hidden = this.showNavItem(children) && hidden;
            }
        } else {

            hidden = !this.checkPermission(item.claim);

            if (!hidden && item.apenasParaUsuarioExterno) {
                hidden = (this.user["interno"] === "true" || this.user["interno"] == true);
            }
        }



        item.hidden = hidden;
        return hidden;
    }

    private checkPermission(claim: FuseClaim): boolean {
        if (claim) {
            //let user = this.oauthService.getIdentityClaims();
            if (this.user[claim.claimType]) {
                if (_.isArray(this.user[claim.claimType])) {
                    return _.includes(this.user[claim.claimType], claim.claimValue);

                }
                return this.user[claim.claimType] == claim.claimValue;

            }
            return false
        }
        return true;
    }
}
