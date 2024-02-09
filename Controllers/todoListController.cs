using Microsoft.AspNetCore.Mvc;
using todoList.Models;
using todoList.Interfaces;

namespace myTodoLists.Controllers;

[ApiController]
[Route("[controller]")]
public class todoListController : ControllerBase
{
    ITodolistService todolistService;
    public todoListController(ITodolistService todolistService)
    {
        this.todolistService = todolistService;
    }
    [HttpGet]
        public ActionResult<List<task>> GetAll() =>
            todolistService.GetAll();
    [HttpGet("{id}")]
    public ActionResult<task> GetById(int id)
    {
        var Task = todolistService.GetById(id);
        if (Task == null)
            return NotFound();
        return Task;
    }

    [HttpPost]
    public IActionResult Create(task newTask)
    {
        todolistService.Add(newTask);
        return CreatedAtAction(nameof(Create), new {id = newTask.Id}, newTask);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id,task newTask)
    {
        if(id != newTask.Id)
            return BadRequest();
        
        var existingTodoList = todolistService.GetById(id);
        if (existingTodoList is null)
        {
            return NotFound();
        }
        todolistService.Update(newTask.Id,newTask);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var Task = todolistService.GetById(id);
        if(Task is null)
            return NotFound();
        
        todolistService.Delete(id);

        return NoContent();
    }
}

