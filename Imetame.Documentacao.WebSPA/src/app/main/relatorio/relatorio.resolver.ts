import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { PaginatedResponse } from 'app/models/PaginatedResponse';
import { Colaborador } from 'app/models/Colaborador';
import { ColaboradorProtheusModel } from 'app/models/DTO/ColaboradorModel';
import { forkJoin } from 'rxjs';
import { AtividadeEspecifica } from 'app/models/AtividadeEspecifica';
import { RelatorioService } from './relatorio.service';
import { ChecklistModel } from 'app/models/DTO/RelatorioModel';

// export const colaboradorResolver: ResolveFn<PaginatedResponse<Colaborador>> = (route, state) => {
//   const router = inject(Router);
//   const service = inject(ColaboradorService); // Injetando serviço 
//   return service.GetAllPaginated().pipe(
//     catchError(error => {
//       router.navigate(['generic-error']);
//       throw error;
//     })
//   );

// };

export const checklistListResolver: ResolveFn<ChecklistModel[]> = (route, state) => {
  const router = inject(Router);
  const service = inject(RelatorioService);
  const idProcesso = route.paramMap.get('idProcesso');
  // Dados de uma semana
  return service.GetDadosCheckList(idProcesso).pipe(
    catchError(error => {
      router.navigate(['generic-error']);
      throw error;
    })
  );
};
