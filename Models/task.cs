namespace todoList.Models
{
public class Task
{
    public int Id { get; set;}
    public string? name { get; set;}
    public bool IsDone {get; set;}
    public int userID{get; set;}
}
}