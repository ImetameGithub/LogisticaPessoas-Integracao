export interface FuseNavigationItem
{
    id: string;
    title: string;
    type: 'item' | 'group' | 'collapsable';
    translate?: string;
    icon?: string;
    hidden?: boolean;
    url?: string;
    classes?: string;
    exactMatch?: boolean;
    externalUrl?: boolean;
    openInNewTab?: boolean;
    function?: any;
    badge?: {
        title?: string;
        translate?: string;
        bg?: string;
        fg?: string;
    };
    children?: FuseNavigationItem[];
    claim?: FuseClaim;
    apenasParaUsuarioExterno?: boolean
}

export interface FuseNavigation extends FuseNavigationItem
{
    children?: FuseNavigationItem[];
}

export interface FuseClaim {
    claimType: string;
    claimValue: string;
}
