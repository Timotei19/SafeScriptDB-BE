namespace Models.DTOs
{
    public class UserStatisticsReponse
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public int SuccessScripts { get; set; }
        public int FailedScripts { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
