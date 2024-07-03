import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GrupoListComponent } from './grupo-list/grupo-list.component';
import { GrupoRoutingModule } from './grupo-routing.module';
import { SharedModule } from 'app/shared/shared.module';
import { GrupoFormComponent } from './grupo-form/grupo-form.component';
import { GrupoService } from './grupo.service';


@NgModule({
    declarations: [
        GrupoListComponent,
        GrupoFormComponent],
  imports: [
      CommonModule,
      GrupoRoutingModule,
      SharedModule
    ],
    providers: [GrupoService],
})
export class GrupoModule { }
