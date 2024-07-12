import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { catchError } from 'rxjs/operators';

import { PaginatedResponse } from 'app/models/PaginatedResponse';
import { ColaboradorService } from './colaboradores.service';
import { AtividadeEspecificaService } from '../atividade-especifica/atividade-especifica.service';
import { Colaborador } from 'app/models/Colaborador';
import { ColaboradorModel, ColaboradorProtheusModel } from 'app/models/DTO/ColaboradorModel';
import { forkJoin } from 'rxjs';
import { AtividadeEspecifica } from 'app/models/AtividadeEspecifica';

export const colaboradorResolver: ResolveFn<PaginatedResponse<Colaborador>> = (route, state) => {
  const router = inject(Router);
  const service = inject(ColaboradorService); // Injetando serviço 
  return service.GetAllPaginated().pipe(
    catchError(error => {
      router.navigate(['generic-error']);
      throw error;
    })
  );

};

export const listAllcolaboradorResolver: ResolveFn<{colaboradorPaginated: PaginatedResponse<Colaborador>,listAllColaborador:ColaboradorProtheusModel[], listAllAtividades: AtividadeEspecifica[]}> = (route, state) => {
  const router = inject(Router);
  const service = inject(ColaboradorService);
  const serviceAtividade = inject(AtividadeEspecificaService);
  const id = route.paramMap.get('id');
  // Dados de uma semana
  return forkJoin({
    colaboradorPaginated: service.GetAllPaginated(),
    listAllColaborador: service.GetAll(),
    listAllAtividades: serviceAtividade.GetAll(),
  }).pipe(
    catchError(error => {
      router.navigate(['generic-error']);
      throw error;
    })
  );
};
