using todoList.Interfaces;
using System.Text.Json;
using System.Text;
using todoList.Models;


namespace todoList.Services;

public class UserServices : IUserService
{
    private readonly List<Models.User> Users;
    private readonly string fileName;

    public UserServices(/*IWebHostEnvinronment webHost*/)
    {
        this.fileName = Path.Combine(/*webHost.ContentRootPath*/ "Data", "user.json");

        using var jsonFile = File.OpenText(fileName);
        Users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    //Save changes in the json users file.
    private void SaveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(Users));
    }

    //Get all users.
    public List<Models.User> GetAll() => Users;

    //Get user by id.
    public Models.User? GetById(int id)
    {
        return Users.FirstOrDefault(t => t.Id == id);
    }

    //Add new user.
    public int Add(Models.User newUser)
    {
        if (Users.Count == 0)
        {
            newUser.Id = 1;
        }
        else
        {
            newUser.Id = Users.Max(t => t.Id) + 1;
        }
        Users.Add(newUser);
        SaveToFile();

        return newUser.Id;
    }

    //Update user.
    public bool Update(Models.User newUser)
    {
        var existingUser = GetById(newUser.Id);
        if (existingUser == null)
            return false;

        var index = Users.IndexOf(existingUser);
        if (index == -1)
            return false;

        Users[index] = newUser;
        SaveToFile();


        return true;
    }

    //Delete user.
    public bool Delete(int id)
    {
        var existingUser = GetById(id);
        if (existingUser == null)
            return false;

        var index = Users.IndexOf(existingUser);
        if (index == -1)
            return false;

        Users.RemoveAt(index);
        SaveToFile();
        return true;
    }

    //Return user by name and password.
    public User? Authentication(string name, string Password)
    {
        return Users.FirstOrDefault(t => t.Username == name && t.Password == Password);
    }

}

public static class UserUtils
{
    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserServices>();
    }
}
