using Microsoft.AspNetCore.Mvc;
using todoList.Models;
using todoList.Interfaces;

namespace myTodoLists.Controllers;

[ApiController]
[Route("[controller]")]
public class userController : ControllerBase
{
    IUserService userService;
    public userController(IUserService userService)
    {
        this.userService = userService;
    }
    [HttpGet]
        public ActionResult<List<todoList.Models.User>> GetAll() =>
            userService.GetAll();
    [HttpGet("{id}")]
    public ActionResult<todoList.Models.User> GetById(int id)
    {
        var User = userService.GetById(id);
        if (User == null)
            return NotFound();
        return User;
    }

    [HttpPost]
    public IActionResult Create(todoList.Models.User newUser)
    {
        userService.Add(newUser);
        return CreatedAtAction(nameof(Create), new {id = newUser.Id}, newUser);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, todoList.Models.User newUser)
    {
        if(id != newUser.Id)
            return BadRequest();
        
        var existingTodoList = userService.GetById(id);
        if (existingTodoList is null)
        {
            return NotFound();
        }
        userService.Update(newUser.Id,newUser);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var User = userService.GetById(id);
        if(User is null)
            return NotFound();
        
        userService.Delete(id);

        return NoContent();
    }
}

