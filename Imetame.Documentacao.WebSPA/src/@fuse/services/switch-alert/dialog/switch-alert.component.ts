import { NgClass, NgIf } from '@angular/common';
import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { FuseSwitchAlertConfig } from '@fuse/services/switch-alert/switch-alert.types';

@Component({
    selector     : 'fuse-switch-alert-dialog',
    templateUrl  : './switch-alert.component.html',
    styleUrls  : ['./switch-alert.component.scss'],
    encapsulation: ViewEncapsulation.None,
    standalone   : true,
    imports      : [NgIf, MatButtonModule, MatDialogModule, MatIconModule, NgClass,FlexLayoutModule],
})
export class FuseSwitchAlertDialogComponent
{
    /**
     * Constructor
     */
    constructor(@Inject(MAT_DIALOG_DATA) public data: FuseSwitchAlertConfig)
    {
    }

}
