import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'app/guard/AuthGuard';
import { CredenciadoraComponent } from './credenciadora/credenciadora.component';
import { ColaboradoresComponent } from './colaboradores/colaboradores.component';
import { AutomacaoDeProcessosService } from './automacao-de-processos.service';


const routes: Routes = [
    
    {
        path: 'cadastro-de-colaboradores',
        component: CredenciadoraComponent,
        
    },
    {
        path: 'cadastro-de-colaboradores/:processamento',
        component: ColaboradoresComponent,
        resolve: { data: AutomacaoDeProcessosService }

    },
    {
        path: 'cadastro-de-colaboradores/:processamento/view',
        component: ColaboradoresComponent,
        resolve: { data: AutomacaoDeProcessosService }

    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    
})
export class AutomacaoDeProcessosRoutingModule { }
