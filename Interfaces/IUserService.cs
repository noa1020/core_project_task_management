using todoList.Models;
using System.Collections.Generic;

namespace todoList.Interfaces
{
    public interface IUserService
    {
        List<Models.User> GetAll();
        bool Delete(int id);
         bool Update( Models.User newUser);
        Models.User? GetById(int id) ;
        int Add(Models.User newUser);
        public User Authentication(string name,string Password);

       }
}