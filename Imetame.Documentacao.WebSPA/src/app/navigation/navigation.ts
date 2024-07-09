import { FuseNavigation } from "@fuse/types";

export const navigation: FuseNavigation[] = [
    {
        id: "root",
        title: "Automações",
        type: "group",
        icon: "apps",
        children: [
            {
                id: "cad-col",
                title: "Cadastro de colaboradores",
                type: "item",
                url: "/automacoes/cadastro-de-colaboradores",
                icon: "business",
            },
            {
                id: "atividade-especificas",
                title: "Atividades específicas",
                type: "item",
                url: "/atividade-especifica",
                icon: "business",
            },
            {
                id: "de-para",
                title: "De Para",
                type: "item",
                url: "/de-para",
                icon: "business",
            },
            {
                id: "pedido",
                title: "Pedido",
                type: "item",
                url: "/pedido",
                icon: "business",
            },
            {
                id: "credenciadora",
                title: "Credenciadora",
                type: "item",
                url: "/credenciadora",
                icon: "business",
            },
            {
                id: "processamento",
                title: "Processamento",
                type: "item",
                url: "/processamento",
                icon: "business",
            },
        ],
    },
];
