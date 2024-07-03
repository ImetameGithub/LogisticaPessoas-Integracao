import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthGuard } from "./guard/AuthGuard";
import { SigninOidcComponent } from "./signin-oidc.component";
import { HomeComponent } from "./main/components/home/home.component";

const routes: Routes = [
    {
        path: "",
        pathMatch: "full",
        redirectTo: "home",
    },
    {
        path: "automacoes",
        loadChildren: () =>
            import(
                "./main/automacao-de-processos/automacao-de-processos.module"
            ).then((mod) => mod.AutomacaoDeProcessosModule),
    },
    {
        path: "de-para",
        loadChildren: () =>
            import("./main/de-para/de-para.module").then(
                (mod) => mod.DeParaModule
            ),
    },

    {
        path: "pedido",
        loadChildren: () =>
            import("./main/pedido/pedido.module").then(
                (mod) => mod.PedidoModule
            ),
    },
    {
        path: "processamento",
        loadChildren: () =>
            import("./main/processamento/processamento.module").then(
                (mod) => mod.ProcessamentoModule
            ),
    },

    {
        path: "nao-encontrado",
        loadChildren: () =>
            import("./main/components/errors/404/error-404.module").then(
                (mod) => mod.Error404Module
            ),
    },
    {
        path: "desafio-interno",
        loadChildren: () =>
            import("./main/components/errors/500/error-500.module").then(
                (mod) => mod.Error500Module
            ),
    },
    {
        path: "negado",
        loadChildren: () =>
            import("./main/components/errors/401/error-401.module").then(
                (mod) => mod.Error401Module
            ),
    },
    {
        path: "home",
        component: HomeComponent,
    },
    {
        path: "signin-oidc",
        component: SigninOidcComponent,
    },

    {
        path: "**",
        redirectTo: "nao-encontrado",
    },
];

@NgModule({
    declarations: [SigninOidcComponent],
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule, SigninOidcComponent],
})
export class AppRoutingModule {}
