import { MatPaginatorIntl } from '@angular/material/paginator';
import { Injectable } from '@angular/core';

@Injectable()
export class CustomMatPaginatorIntl extends MatPaginatorIntl {
    constructor() {
        super();
        this.firstPageLabel = 'Primeira página';
        this.itemsPerPageLabel = 'Itens por página';
        this.lastPageLabel = 'Última página';
        this.nextPageLabel = 'Próxima página';
        this.previousPageLabel = 'Página anterior';
        //this.changes.next();
    }



    getRangeLabel = (page: number, pageSize: number, length: number) => {
        if (length === 0 || pageSize === 0) {
            return `0 / ${length}`;
        }
        length = Math.max(length, 0);
        const startIndex = page * pageSize;
        const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
        return `${startIndex + 1} - ${endIndex} / ${length}`;
    }
}