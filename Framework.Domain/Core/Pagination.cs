namespace Framework.Domain.Core
{
    public class Pagination
    {
        public int PageTotal { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
        public int EndPage { get; set; }
        public int StartPage { get; set; }
        public int totalPages { get; set; } 
        public int totalItems { get; set; }
        public int[] pages { get; set; }
    }
}
