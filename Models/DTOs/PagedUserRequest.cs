namespace Models.DTOs
{
    public class PagedUserRequest
    {
        public string? FilterText { get; set; } = "";
        public string? Email { get; set; } = null;
        public string? UserName { get; set; } = null;
        public int UserId { get; set; } = 0;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Email";
        public bool SortDesc { get; set; } = false;
    }
}
