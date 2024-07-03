import { Pipe, PipeTransform } from '@angular/core';

// tslint:disable-next-line:use-pipe-transform-interface
@Pipe({
    name: 'uppercase'
})
export class UppercasePipe implements PipeTransform {
    public transform(value: string) {
        return value.toUpperCase();
    }
}
