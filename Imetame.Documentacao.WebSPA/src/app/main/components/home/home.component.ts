import { Component, ViewEncapsulation } from '@angular/core';

import { FuseConfigService } from '@fuse/services/config.service';
import { fuseAnimations } from '@fuse/animations';

@Component({
    selector     : 'home',
    templateUrl  : './home.component.html',
    styleUrls    : ['./home.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations   : fuseAnimations
})
export class HomeComponent
{
    /**
     * Constructor
     *
     * @param {FuseConfigService} _fuseConfigService
     */
    constructor(
        private _fuseConfigService: FuseConfigService
    )
    {
        // Configure the layout
        //this._fuseConfigService.config = {
        //    layout: {
        //        navbar   : {
        //            hidden: true
        //        },
        //        toolbar  : {
        //            hidden: true
        //        },
        //        footer   : {
        //            hidden: true
        //        },
        //        sidepanel: {
        //            hidden: true
        //        }
        //    }
        //};
    }
}
