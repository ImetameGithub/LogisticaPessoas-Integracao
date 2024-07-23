import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { AutomacaoDeProcessosService } from '../automacao-de-processos.service';
import { MatDialog } from '@angular/material/dialog';
import { ShowLogDialogComponent } from '../show-log-dialog/show-log-dialog.component';
import { ActivatedRoute } from '@angular/router';

export interface Finalizado {
  numCad: string;
  nome: string;
  funcao: string;
  cracha: string;
  equipe: string;
}

@Component({
  selector: 'app-finalizados',
  templateUrl: './finalizados.component.html',
  styleUrls: ['./finalizados.component.scss']
})
export class FinalizadosComponent {
  displayedColumns: string[] = ['numCad', 'nome', 'funcao', 'cracha', 'equipe', 'buttons'];
  dataSource: any;
  private _subs = new Subscription();
  private updateInterval: any;

  constructor(private finalizadosService: AutomacaoDeProcessosService, public dialog: MatDialog,  private route: ActivatedRoute) { }

//   ngOnInit() {
//     const idProcessamento = this.route.snapshot.paramMap.get('processamento');
//     this._subs.add(
//       this.finalizadosService.finalizados$.subscribe(finalizados => {
//         console.log(finalizados)
//         this.dataSource = finalizados;
//       })
//     );
//     this.finalizadosService.buscarEAdicionarResultados(idProcessamento)
//     .then((repsonse) => console.log('Resultados adicionados', repsonse))
//     .catch(error => console.error('Erro ao adicionar resultados', error));
//   }

  
ngOnInit() {
    // this.watchProcessamentoId();
  }

watchProcessamentoId() {
    this._subs.add(
      this.route.paramMap.subscribe(params => {
        const idProcessamento = params.get('processamento');

        if (idProcessamento) {
          this.finalizadosService.limparFinalizados();

          // this.finalizadosService.iniciarObservacaoFinalizados(idProcessamento);
          this.finalizadosService.buscarEAdicionarResultados(idProcessamento)
            .then(() => {}).catch(error => {
              console.error('Erro ao atualizar finalizados:', error);
            });

          this._subs.add(
            this.finalizadosService.finalizados$.subscribe((finalizados: any) => {
              this.dataSource = finalizados;
            })
          );
        } else {
          console.error('ID do processo não encontrado na URL.');
        }
      })
    );
  }

  ngOnDestroy() {
    if (this.updateInterval) {
      clearInterval(this.updateInterval);
    }
    this._subs.unsubscribe();
  }

  detalhar(item) {
    let logDialogRef = this.dialog.open(ShowLogDialogComponent);
    logDialogRef.componentInstance.logs = item.log;
  }
}
