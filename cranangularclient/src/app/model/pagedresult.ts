export class PagedResult<T> {
    public data: T[] = [];
    public currentPage: number;
    public numpages: number;
    public pagesize: number;
}
