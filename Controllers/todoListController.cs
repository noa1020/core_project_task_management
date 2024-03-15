using Microsoft.AspNetCore.Mvc;
using todoList.Models;
using todoList.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace myTodoLists.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoListController(ITodolistService todolistService) : ControllerBase
{
    readonly ITodolistService todolistService = todolistService;

    [HttpGet]
    [Authorize(Policy = "User")]
    public ActionResult<List<todoList.Models.Task>> GetAll()
    {
        var id = User.FindFirst("id")?.Value;
        if (id is null)
            return NotFound();

        return todolistService.GetAll(Convert.ToInt32(id));
    }


    [HttpGet("{id}")]
    [Authorize(Policy = "User")]
    public ActionResult<todoList.Models.Task> GetById(int id)
    {
        var userID = User.FindFirst("id")?.Value;
        if (userID is null)
            return NotFound();
        var Task = todolistService.GetById(id, Convert.ToInt32(userID));
        if (Task == null)
            return NotFound();
        return Task;
    }

    [HttpPost]
    [Authorize(Policy = "User")]
    public IActionResult Create(todoList.Models.Task newTask)
    {
        var userID = User.FindFirst("id")?.Value;
        if (userID is null)
            return NotFound();
        newTask.userID = Convert.ToInt32(userID);
        todolistService.Add(newTask);
        return CreatedAtAction(nameof(Create), new { id = newTask.Id }, newTask);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "User")]

    public IActionResult Update(int id, todoList.Models.Task newTask)
    {
        if (id != newTask.Id)
            return BadRequest();
        var userID = User.FindFirst("id")?.Value;
        if (userID is null)
            return NotFound();
        var currentTask = todolistService.GetById(id,Convert.ToInt32(userID));
        if (currentTask is null)
        {
            return NotFound();
        }
        newTask.userID=Convert.ToInt32(userID);
        todolistService.Update(newTask.Id, newTask);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "User")]

    public IActionResult Delete(int id)
    {
        var userID = User.FindFirst("id")?.Value;
        if (userID is null)
            return NotFound();
        var Task = todolistService.GetById(id,Convert.ToInt32(userID));
        if (Task is null)
            return NotFound();

        todolistService.Delete(id);

        return NoContent();
    }
}

