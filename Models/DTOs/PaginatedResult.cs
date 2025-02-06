namespace Models.DTOs
{
    public class PagedResult<T>
    {
        public List<T> Records { get; set; }
        public int TotalRecords { get; set; }
    }
}
