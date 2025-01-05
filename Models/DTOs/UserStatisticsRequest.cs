namespace Models.DTOs
{
    public class UserStatisticsRequest
    {
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
