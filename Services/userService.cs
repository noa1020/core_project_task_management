using todoList.Models;
using Microsoft.Extensions.DependencyInjection;
using todoList.Interfaces;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.IO;
using System;
using todoList.Services;



namespace todoList.Services;

public  class userServices:IUserService
{
    private  List<Models.User> Users;
    private string fileName ;

        public userServices(/*IWebHostEnvinronment webHost*/)
        {
            this.fileName = Path.Combine(/*webHost.ContentRootPath*/ "Data", "user.json");

            using (var jsonFile = File.OpenText(fileName))
            {
            Users = JsonSerializer.Deserialize <List<Models.User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(Users));
        }



    public  List<Models.User> GetAll() => Users;

    public Models.User GetById(int id) 
    {
        return Users.FirstOrDefault(t => t.Id == id);
    }

    public  int Add(Models.User newUser)
    {
        if (Users.Count == 0)

            {
                newUser.Id = 1;
            }
            else
            {
        newUser.Id =  Users.Max(t => t.Id) + 1;

            }

        Users.Add(newUser);
        saveToFile();

        return newUser.Id;
    }
  
    public  bool Update(int id, Models.User newUser)
    {
        if (id != newUser.Id)
            return false;

        var existingUser = GetById(id);
        if (existingUser == null )
            return false;

        var index = Users.IndexOf(existingUser);
        if (index == -1 )
            return false;

        Users[index] = newUser;
        saveToFile();


        return true;
    }  

      
    public  bool Delete(int id)
    {
         var existingUser = GetById(id);
        if (existingUser == null )
            return false;

        var index = Users.IndexOf(existingUser);
        if (index == -1 )
            return false;

        Users.RemoveAt(index);
        saveToFile();
        return true;
    }  



}
public static class UserUtils
{
    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, userServices>();
    }
}