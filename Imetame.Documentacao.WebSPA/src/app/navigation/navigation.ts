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
                title: "Cadastro",                
                type: 'collapsable',                
                icon: "list",  
                children: [
                    {
                        id: "cad-col",
                        title: "Cadastro de Colaboradores",
                        type: "item",
                        url: "/automacoes/cadastro-de-colaboradores",                        
                        icon: "label_important",                
                    },
                    {
                        id: "pedido",
                        title: "Pedido",
                        type: "item",
                        url: "/pedido",
                        icon: "label_important",
                    },
                ]              
            },
            {
                id: "cad-col",
                title: "Configurações",                
                type: 'collapsable',                
                icon: "list",  
                children: [
                    {
                        id: "atividade-especificas",
                        title: "Atividades específicas",
                        type: "item",
                        url: "/atividade-especifica",
                        icon: "label_important",
                    },            
                    {
                        id: "colaboradores",
                        title: "Colaboradores",
                        type: "item",
                        url: "/colaboradores",
                        icon: "label_important",
                    },
                    {
                        id: "documento",
                        title: "Documentos",
                        type: "item",
                        url: "/documento",
                        icon: "label_important",
                    },
                    // {
                    //     id: "de-para",
                    //     title: "De Para",
                    //     type: "item",
                    //     url: "/de-para",
                    //     icon: "label_important",
                    // },
                    {
                        id: "credenciadora",
                        title: "Credenciadora",
                        type: "item",
                        url: "/credenciadora",
                        icon: "label_important",
                    },
                    {
                        id: "processamento",
                        title: "Processamento",
                        type: "item",
                        url: "/processamento",
                        icon: "label_important",
                    },
                ]              
            },            
        ],
    },
];
