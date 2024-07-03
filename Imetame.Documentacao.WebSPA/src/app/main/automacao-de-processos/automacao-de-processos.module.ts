import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { AutomacaoDeProcessosService } from './automacao-de-processos.service';
import { AutomacaoDeProcessosRoutingModule } from './automacao-de-processos-routing.module';
import { SharedModule } from 'app/shared/shared.module';
import { CredenciadoraComponent } from './credenciadora/credenciadora.component';
import { ColaboradoresComponent } from './colaboradores/colaboradores.component';
import { ShowLogDialogComponent } from './show-log-dialog/show-log-dialog.component';
import { LogComponent } from './logs/logs.component';
import { FinalizadosComponent } from './finalizados/finalizados.component';
import { PedidoService } from '../pedido/pedido.service';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';



@NgModule({
    declarations: [CredenciadoraComponent, ColaboradoresComponent, ShowLogDialogComponent, LogComponent, FinalizadosComponent],
  imports: [
      CommonModule,
      AutomacaoDeProcessosRoutingModule,
      SharedModule,
      MatButtonModule,
      MatTableModule,
  ],
    providers: [AutomacaoDeProcessosService,PedidoService],
    entryComponents: [ShowLogDialogComponent]

    
})
export class AutomacaoDeProcessosModule { }
