import { inject, Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FuseSwitchAlertConfig } from '@fuse/services/switch-alert/switch-alert.types';
import { FuseSwitchAlertDialogComponent } from './dialog/switch-alert.component';
import { merge } from 'lodash-es';

@Injectable({providedIn: 'root'})
export class FuseSwitchAlertService
{
    private _matDialog: MatDialog = inject(MatDialog);
    private _defaultConfig: FuseSwitchAlertConfig = {
        title      : 'Confirm action',
        message    : 'Are you sure you want to confirm this action?',
        icon       : {
            show : true,
            name : 'warning',
            color: 'warn',
        },
        dismissible: false,
    };

    /**
     * Constructor
     */
    constructor()
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    open(config: FuseSwitchAlertConfig = {}): MatDialogRef<FuseSwitchAlertDialogComponent>
    {
        // Merge the user config with the default config
        const userConfig = merge({}, this._defaultConfig, config);

        // Open the dialog
        return this._matDialog.open(FuseSwitchAlertDialogComponent, {
            autoFocus   : false,
            disableClose: !userConfig.dismissible,
            data        : userConfig,
            panelClass  : 'fuse-switch-alert-dialog-panel',
        });
    }
}
