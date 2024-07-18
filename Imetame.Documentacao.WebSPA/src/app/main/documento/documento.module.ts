import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocumentoListComponent } from './documento-list/documento-list.component';
import { DocumentoFormComponent } from './documento-form/documento-form.component';
import { DocumentoService } from './documento.service';
import { SharedModule } from 'app/shared/shared.module';
import { MatDialogModule } from '@angular/material/dialog';
import { DeParaRoutingModule } from './documento-routing.module';
import { CustomSearchSelectComponent } from 'app/shared/components/custom-select/custom-select.component';



@NgModule({
  declarations: [
    DocumentoListComponent,
    DocumentoFormComponent
  ],
  imports: [CommonModule, DeParaRoutingModule, SharedModule, MatDialogModule,CustomSearchSelectComponent,],
    providers: [DocumentoService],
})
export class DocumentoModule { }
