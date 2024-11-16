namespace Data_Access_Layer.Repositories
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Roles { get; set; }
    }
}