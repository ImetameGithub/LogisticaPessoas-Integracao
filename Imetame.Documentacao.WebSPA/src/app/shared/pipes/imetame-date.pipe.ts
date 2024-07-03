import { Pipe, PipeTransform } from '@angular/core';

// tslint:disable-next-line:use-pipe-transform-interface
@Pipe({
    name: 'imetame_date'
})
export class ImetameDatePipe implements PipeTransform {
    public transform(value: string) {
        if (value == null || value == undefined || value.length != 8)
            return value;

        return value.substring(6) + "/" + value.substring(4, 6) + "/" + value.substring(0, 4);
    }
}
