import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AtividadeEspecificaListComponent } from './atividade-especifica-list/atividade-especifica-list.component';
import { AtividadeEspecificaFormComponent } from './atividade-especifica-form/atividade-especifica-form.component';
import { AtividadeEspecificaRoutingModule } from './atividade-especifica-routing.module';
import { MatDialogModule } from '@angular/material/dialog';
import { SharedModule } from 'app/shared/shared.module';



@NgModule({
  declarations: [
    AtividadeEspecificaListComponent,
    AtividadeEspecificaFormComponent
  ],
  imports: [
    CommonModule,
    AtividadeEspecificaRoutingModule, 
    SharedModule, 
    MatDialogModule
  ]
})
export class AtividadeEspecificaModule { }
