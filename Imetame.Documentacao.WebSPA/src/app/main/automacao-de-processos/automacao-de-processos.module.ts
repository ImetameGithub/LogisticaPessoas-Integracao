import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';


import { AutomacaoDeProcessosService } from './automacao-de-processos.service';
import { AutomacaoDeProcessosRoutingModule } from './automacao-de-processos-routing.module';
import { SharedModule } from 'app/shared/shared.module';
import { CredenciadoraComponent } from './credenciadora/credenciadora.component';
import { ColaboradoresComponent } from './colaboradores/colaboradores.component';
import { ShowLogDialogComponent } from './show-log-dialog/show-log-dialog.component';
import { FinalizadosComponent } from './finalizados/finalizados.component';
import { PedidoService } from '../pedido/pedido.service';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { DocumentosModalComponent } from './colaboradores/documentos-modal/documentos-modal.component';
import {MatGridListModule} from '@angular/material/grid-list';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { CustomSearchSelectComponent } from 'app/shared/components/custom-select/custom-select.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LogComponent } from './logs/logs.component';
import { MatSortModule } from '@angular/material/sort';



@NgModule({
    declarations: [CredenciadoraComponent, ColaboradoresComponent, ShowLogDialogComponent, LogComponent, FinalizadosComponent, DocumentosModalComponent],
  imports: [
      CommonModule,
      AutomacaoDeProcessosRoutingModule,
      MatFormFieldModule,
      MatDatepickerModule,
      MatNativeDateModule,
      CustomSearchSelectComponent,
      DatePipe,
      SharedModule,
      MatGridListModule,
      MatButtonModule,
      MatTableModule,
      MatProgressSpinnerModule,
      MatSortModule
  ],
    providers: [AutomacaoDeProcessosService,PedidoService],
    entryComponents: [ShowLogDialogComponent]

    
})
export class AutomacaoDeProcessosModule { }
