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
public class TodoListServices : ITodolistService
{
    private List<Models.Task> tasks;
    private readonly string fileName;

    public TodoListServices(/*IWebHostEnvinronment webHost*/)
    {
        this.fileName = Path.Combine(/*webHost.ContentRootPath*/ "Data", "todoList.json");

        using var jsonFile = File.OpenText(fileName);
        tasks = JsonSerializer.Deserialize<List<Models.Task>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    private void saveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(tasks));
    }

    public List<Models.Task> GetAll(int userID)
    {
        return tasks.FindAll(task => task.userID == userID);
    }
    public Models.Task? GetById(int id)
    {
        return tasks?.FirstOrDefault(t => t?.Id == id);
    }
    public Models.Task? GetById(int id, int userID)
    {
        return tasks?.FirstOrDefault(t => t?.Id == id && t.userID == userID);
    }

    public int Add(Models.Task newTask)
    {
        if (tasks.Count == 0)

        {
            newTask.Id = 1;
        }
        else
        {
            newTask.Id = tasks.Max(t => t.Id) + 1;

        }

        tasks.Add(newTask);
        saveToFile();

        return newTask.Id;
    }

    public bool Update(int id, Models.Task newTask)
    {
        if (id != newTask.Id)
            return false;

        var existingTask = GetById(id);
        if (existingTask == null)
            return false;

        var index = tasks.IndexOf(existingTask);
        if (index == -1)
            return false;

        tasks[index] = newTask;
        saveToFile();


        return true;
    }


    public bool Delete(int id)
    {
        var existingTask = GetById(id);
        if (existingTask == null)
            return false;

        var index = tasks.IndexOf(existingTask);
        if (index == -1)
            return false;

        tasks.RemoveAt(index);
        saveToFile();
        return true;
    }
    public void DeleteByUserId(int userID)
    {
        tasks.RemoveAll(task => task.userID == userID);
    }

}
public static class TaskUtils
{
    public static void AddTask(this IServiceCollection services)
    {
        services.AddSingleton<ITodolistService, TodoListServices>();
    }
}