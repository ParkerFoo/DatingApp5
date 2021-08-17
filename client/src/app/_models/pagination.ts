export interface Pagination{
    currentPage:number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}


export class PaginatedResult<T>{ //we are planning to use Member array for the T
    result:T; //list of members going to stored here
    pagination:Pagination;
}