export class CustomOptionsSelect {
    display: string;
    value: string | Object;

    constructor(valor : string | Object, display : string) {
        this.display = display;
        this.value = valor;
    }
}
