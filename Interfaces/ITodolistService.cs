using todoList.Models;
using System.Collections.Generic;

namespace todoList.Interfaces
{
    public interface ITodolistService
    {
        List<task> GetAll();
       // int Count ();
        bool Delete(int id);
         bool Update(int id, task newTask);
        task GetById(int id) ;
        int Add(task newTask);
    
       }
}