using todoList.Models;
using System.Collections.Generic;

namespace todoList.Interfaces
{
    public interface ITodolistService
    {
        List<Models.Task> GetAll(int userID);
        bool Delete(int id);
         bool Update(int id, Models.Task newTask);
        Models.Task? GetById(int id,int userID) ;
        Models.Task? GetById(int id) ;
        int Add(Models.Task newTask);
        void DeleteByUserId(int userID);
       }
}