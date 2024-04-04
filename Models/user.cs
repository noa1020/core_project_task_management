namespace todoList.Models
{
    public enum Status { Admin, User };
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public  Status Status { get; set; }

    }
}
