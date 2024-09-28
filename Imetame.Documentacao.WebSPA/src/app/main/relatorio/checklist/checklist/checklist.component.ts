import { Component, ElementRef, OnDestroy, OnInit, ViewChild, ViewEncapsulation, } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { fuseAnimations } from "@fuse/animations";
import { Title } from "@angular/platform-browser";
import { Validators, FormControl, UntypedFormGroup, UntypedFormBuilder, } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { FuseProgressBarService } from "@fuse/components/progress-bar/progress-bar.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { RelatorioService } from "../../relatorio.service";
import { Pedido } from "app/models/Pedido";
import { CustomOptionsSelect } from "app/shared/components/custom-select/components.types";
import * as _ from "lodash";
import { MatAutocomplete, MatAutocompleteSelectedEvent } from "@angular/material/autocomplete";
import { DataService } from "app/services/data.service";
import { AutomacaoDeProcessosService } from "app/main/automacao-de-processos/automacao-de-processos.service";

@Component({
  selector: 'checklist',
  templateUrl: './checklist.component.html',
  styleUrls: ['./checklist.component.scss'],
  encapsulation: ViewEncapsulation.None,
  animations: fuseAnimations,
})
export class ChecklistComponent implements OnInit, OnDestroy {
  isTravarPesquisa: boolean = false;
  OsOptions: CustomOptionsSelect[] = [];
  listPedidos: CustomOptionsSelect[] = [];
  ordemServicoOptions: CustomOptionsSelect[] = [];
  filteredOss: Observable<any[]>;
  oss: string[] = [];
  osCtrl = new FormControl();

  filtroForm: UntypedFormGroup;
  private _unsubscribeAll: Subject<any>;

  @ViewChild('osInput') osInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;

  constructor(
    private titleService: Title,
    private _formBuilder: UntypedFormBuilder,
    private _snackbar: MatSnackBar,
    private _fuseProgressBarService: FuseProgressBarService,
    private _relatorioService: RelatorioService,
    private router: Router,
    private route: ActivatedRoute,
  ) {
    this._unsubscribeAll = new Subject();
    this.filtroForm = this._formBuilder.group({
      oss: [null, [Validators.required]],
      pedido: [null, [Validators.required]],
    });
    this.titleService.setTitle("Checklist");

    this.filteredOss = new Observable();


  }

  ngOnInit(): void {
    this.getOs('')
    this._relatorioService.listPedido$.subscribe(
      (data: Pedido[]) => {
        if (data != null) {
          this.listPedidos = data.map(x => new CustomOptionsSelect(x.Id, `${x.NumPedido} - ${x.Unidade}`));
        }
      },
      (error) => {
        console.error('Erro ao buscar credenciadoras', error);
      }
    );
    this._relatorioService.getOss('').subscribe(
      (ordensServicos: any[]) => {
        this.ordemServicoOptions = ordensServicos.map(item => new CustomOptionsSelect(item.os, item.numero + ' - ' + item.descricao)) ?? [];
      },
      (error) => {
        console.error('Erro ao buscar pedidos', error);
      }
    );
  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next();
    this._unsubscribeAll.complete();
  }

  getOs(searchValue) {
    const codOs = this.filtroForm.get("oss").value;
    this._relatorioService.getOss(searchValue).subscribe(
      (ordensServicos: any[]) => {
        this.OsOptions = ordensServicos.map(item => new CustomOptionsSelect(item.numero, item.numero + ' - ' + item.descricao)) ?? [];
      },
      (error) => {
        console.error('Erro ao buscar pedidos', error);
      }
    );
  }

  searchOs(searchValue) {
    if (!this.isTravarPesquisa && searchValue != '')
      this._relatorioService.getOss(searchValue).subscribe(
        (ordensServicos: any[]) => {
          this.OsOptions = ordensServicos.map(item => new CustomOptionsSelect(item.numero, item.numero + ' - ' + item.descricao)) ?? [];
        },
        (error) => {
          console.error('Erro ao buscar pedidos', error);
        }
      );
    this.isTravarPesquisa = false;
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.oss.push(event.option.value.numero);
    this.osInput.nativeElement.value = '';
    this.osCtrl.setValue(null);
    this.filtroForm.controls.oss.setValue(this.oss);
  }

  displayFnResp(user: any): string {
    return user && user.numero ? user.numero + ' - ' + user.descricao : '';
  }


  avancar() {
    if (!this.filtroForm.valid) {
      return;
    }
    this._fuseProgressBarService.show();
    const idPedido = this.filtroForm.get("pedido").value;
    const codOs = this.filtroForm.get("oss").value;
    const values = this.filtroForm.getRawValue();

    this._relatorioService.getProcessoAtivo({ idPedido: values.pedido }).then(
      (processo) => {
        if (!processo.id) {
          this._fuseProgressBarService.hide();
          this._relatorioService.cadastrarProcessamento({ IdPedido: idPedido, Oss: codOs, OssString: '' }).then(
            (processamento: any) => {
              const idProcesso = processamento.Id;
              this.router.navigate([`checklist/${idProcesso}`], { relativeTo: this.route });                         
            }
          );
        } else {
          this._fuseProgressBarService.hide();
          const idProcesso = processo.id;
          this.router.navigate([`checklist/${idProcesso}`], { relativeTo: this.route });
        }
      },
    );


  }


}
