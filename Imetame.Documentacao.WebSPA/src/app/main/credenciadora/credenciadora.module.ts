import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CredenciadoraListComponent } from './credenciadora-list/credenciadora-list.component';
import { CredenciadoraFormComponent } from './credenciadora-form/credenciadora-form.component';
import { MatDialogModule } from '@angular/material/dialog';
import { SharedModule } from 'app/shared/shared.module';
import { CredenciadoraRoutingModule } from './credenciadora-routing.module';



@NgModule({
  declarations: [
    CredenciadoraListComponent,
    CredenciadoraFormComponent
  ],
  imports: [CommonModule,CredenciadoraRoutingModule, SharedModule, MatDialogModule]
})
export class CredenciadoraModule { }
