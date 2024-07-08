import { NgModule, APP_INITIALIZER, LOCALE_ID } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { MatMomentDateModule } from '@angular/material-moment-adapter';

import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorIntl } from '@angular/material/paginator';



import { TranslateModule } from '@ngx-translate/core';


import { FuseModule } from '@fuse/fuse.module';
import { FuseSharedModule } from '@fuse/shared.module';
import { FuseProgressBarModule, FuseSidebarModule } from '@fuse/components';

import { NgxMaskModule, IConfig } from 'ngx-mask'


import { AppComponent } from 'app/app.component';

import { LayoutModule } from 'app/layout/layout.module';
import { AppRoutingModule } from './app-routing.module';
import { OAuthModule } from 'angular-oauth2-oidc';
import { fuseConfig } from './config/fuseConfig';
import { IDENTITY_URL, API_URL, IDENTITY_API_URL, EXTRANET_API_URL, } from './config/tokens';
import { IDENTITY_URL_VALUE, API_URL_VALUE, EXTRANET_API_URL_VALUE } from './config/urlConfig';
import { HomeComponent } from './main/components/home/home.component';

import { MAT_MOMENT_DATE_FORMATS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';



import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import localePtExtra from '@angular/common/locales/extra/pt';
import { CustomMatPaginatorIntl } from './i18n/custom-mat-paginator-intl';
import { DataService } from './services/data.service';
import { IDENTITY_API_URL_VALUE } from './config/urlConfig';
import { PermissaoService } from './services/permissao.service';
import { ErrorService } from './services/error.service';
import { LuxonDateAdapter } from '@angular/material-luxon-adapter';
import { QuillModule } from 'ngx-quill';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
// import { StatusEnumPipe } from './shared/pipes/status-enum.pipe';





registerLocaleData(localePt, 'pt', localePtExtra);

const maskConfig: Partial<IConfig> = {
    validation: false,
  };

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        AppRoutingModule,
        TranslateModule.forRoot(),
        NgxMaskModule.forRoot(maskConfig),

        // Material moment date module
        MatMomentDateModule,
        

        // Material
        MatButtonModule,
        MatIconModule,
        MatDialogModule ,
        MatSelectModule,
        // Fuse modules
        FuseModule.forRoot(fuseConfig),
        FuseProgressBarModule,
        FuseSharedModule,
        FuseSidebarModule,
        
        //Auth module
        OAuthModule.forRoot({
            resourceServer: {
                allowedUrls: [IDENTITY_API_URL_VALUE, API_URL_VALUE, EXTRANET_API_URL_VALUE],//
                sendAccessToken: true
            }
        }),

        
        // App modules
        LayoutModule,

        QuillModule.forRoot()
        
    ],
    providers: [
        

         // Material Date Adapter
         {
            provide : DateAdapter,
            useClass: LuxonDateAdapter,
        },
        {
            provide : MAT_DATE_FORMATS,
            useValue: {
                parse  : {
                    dateInput: 'D',
                },
                display: {
                    dateInput         : 'dd/MM/yyyy',
                    monthYearLabel    : 'LLL yyyy',
                    dateA11yLabel     : 'DD',
                    monthYearA11yLabel: 'LLLL yyyy',
                },
            },
        },

        { provide: IDENTITY_URL, useValue: IDENTITY_URL_VALUE },
        { provide: API_URL, useValue: API_URL_VALUE },
        { provide: EXTRANET_API_URL, useValue: EXTRANET_API_URL_VALUE },
        { provide: IDENTITY_API_URL, useValue: IDENTITY_API_URL_VALUE },
        { provide: MAT_DATE_LOCALE, useValue: 'pt-BR' },
        { provide: LOCALE_ID, useValue: 'pt' },
        { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
        { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
        { provide: MatPaginatorIntl, useClass: CustomMatPaginatorIntl },
        DataService,
        PermissaoService,
        ErrorService
    ],
    bootstrap   : [
        AppComponent
    ]
})
export class AppModule
{
}
