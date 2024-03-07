using Microsoft.AspNetCore.Mvc;
using todoList.Models;
using todoList.Interfaces;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize(Policy = "User")]
    public ActionResult<List<todoList.Models.Task>> GetAll() =>
            todolistService.GetAll();

    [HttpGet("{id}")]
    [Authorize(Policy = "User")]

    public ActionResult<todoList.Models.Task> GetById(int id)
    {
        var Task = todolistService.GetById(id);
        if (Task == null)
            return NotFound();
        return Task;
    }

    [HttpPost]
    [Authorize(Policy = "User")]
    public IActionResult Create(todoList.Models.Task newTask)
    {
        todolistService.Add(newTask);
        return CreatedAtAction(nameof(Create), new { id = newTask.Id }, newTask);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "User")]

    public IActionResult Update(int id, todoList.Models.Task newTask)
    {
        if (id != newTask.Id)
            return BadRequest();

        var existingTodoList = todolistService.GetById(id);
        if (existingTodoList is null)
        {
            return NotFound();
        }
        todolistService.Update(newTask.Id, newTask);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "User")]

    public IActionResult Delete(int id)
    {
        var Task = todolistService.GetById(id);
        if (Task is null)
            return NotFound();

        todolistService.Delete(id);

        return NoContent();
    }
}

