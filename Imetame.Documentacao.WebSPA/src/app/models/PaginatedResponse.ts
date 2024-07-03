export class PaginatedResponse<T> {
    totalCount: number;
    page: number;
    pageSize: number;
    data: T[];
}
  