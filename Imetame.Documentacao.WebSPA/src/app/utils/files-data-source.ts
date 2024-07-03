import { DataSource } from '@angular/cdk/collections';
import { Observable } from 'rxjs';


export class FilesDataSource<T> extends DataSource<T>
{
    constructor(private service: any) {
        super();
    }

    get count(): number {
        return this.service.count;
    }

    get pageIndex(): number {
        return this.service.pageIndex;
    }

    get pageSize(): number {
        return this.service.pageSize;
    }

    /** Connect function called by the table to retrieve one stream containing the data to render. */
    connect(): Observable<T[]> {
        return this.service.onItensChanged;
    }

    disconnect() {
    }
}