namespace Data_Access_Layer.Repositories
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}