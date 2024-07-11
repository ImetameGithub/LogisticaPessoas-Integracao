import { Component, OnInit } from '@angular/core';
import { CustomOptionsSelect } from 'app/shared/components/custom-select/components.types';

@Component({
  selector: 'colaboradores-filtro',
  templateUrl: './colaboradores-filtro.component.html',
  styleUrls: ['./colaboradores-filtro.component.scss']
})
export class ColaboradoresFiltroComponent implements OnInit {

  listPerfis: string []
  pendenteCad = false;
  mudaFuncao = false;
  atividadesOptions: CustomOptionsSelect[] = [];
  colaboradoresOptions: CustomOptionsSelect[] = [];
  

  constructor() { }

  ngOnInit(): void {

    // this.colaboradoresOptions = listPerfis.map(estrutura => new CustomOptionsSelect(estrutura, estrutura)) ?? [];
    // this.atividadesOptions = listPerfis.map(estrutura => new CustomOptionsSelect(estrutura, estrutura)) ?? [];

  }

}
