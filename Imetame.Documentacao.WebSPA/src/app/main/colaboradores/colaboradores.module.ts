import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ColaboradoresListComponent } from './colaboradores-list/colaboradores-list.component';
import { MatDialogModule } from '@angular/material/dialog';
import { SharedModule } from 'app/shared/shared.module';
import { ColaboradorRoutingModule } from './colaboradores-routing.module';
import { ColaboradoresFormComponent } from './colaboradores-form/colaboradores-form.component';
import { ColaboradoresAtividadeModalComponent } from './colaboradores-atividade-modal/colaboradores-atividade-modal.component';
import { CustomSearchSelectComponent } from 'app/shared/components/custom-select/custom-select.component';



@NgModule({
  declarations: [
    ColaboradoresListComponent,
    ColaboradoresFormComponent,
    ColaboradoresAtividadeModalComponent
  ],
  imports: [CommonModule, ColaboradorRoutingModule, SharedModule, MatDialogModule, CustomSearchSelectComponent,],
})
export class ColaboradoresModule { }
