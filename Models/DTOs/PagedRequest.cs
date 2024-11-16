namespace Models.DTOs
{
    public class PagedRequest
    {
        public string FilterText { get; set; } = "";
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "startDate";
        public bool SortDesc { get; set; } = false;
    }
}
