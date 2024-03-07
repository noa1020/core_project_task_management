using Microsoft.AspNetCore.Mvc;
using todoList.Models;
using todoList.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using todoList.Services;
namespace myTodoLists.Controllers;

[ApiController]
[Route("[controller]")]
public class userController : ControllerBase
{
    ITodolistService todolistService;
    IUserService userService;
    public userController(IUserService userService ,ITodolistService todolistService)
    {
        this.userService = userService;
        this.todolistService=todolistService;
    }
    [HttpPost]
    [Route("/login")]
    public ActionResult<String> Login( string name,string Password)
    {
        if (name == "admin" && Password == "bbb")
        {

            var claims = new List<Claim>
            {
                new Claim("type", "Admin"),
                new Claim("id","1"),
            };

            var token = TokenService.GetToken(claims);

            return new OkObjectResult(TokenService.WriteToken(token));
        }
        int userId=userService.Authentication(name,Password);
        if ( userId!=null){
            var claims = new List<Claim>
            {
                new Claim("type", "User"),
                new Claim("id", userId.ToString()),
            };
            var token = TokenService.GetToken(claims);
            return new OkObjectResult(TokenService.WriteToken(token));
        }
        return Unauthorized();
    }


    [HttpGet]
    [Authorize(Policy = "Admin")]
    public ActionResult<List<todoList.Models.User>> GetAll() =>
            userService.GetAll();
    [HttpGet("{id}")]
    [Authorize(Policy = "User")]
    public ActionResult<todoList.Models.User> GetById(int id)
    {
        var User = userService.GetById(id);
        if (User == null)
            return NotFound();
        return User;
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public IActionResult Create(todoList.Models.User newUser)
    {
        userService.Add(newUser);
        return CreatedAtAction(nameof(Create), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "User")]
    public IActionResult Update(int id, todoList.Models.User newUser)
    {
        if (id != newUser.Id)
            return BadRequest();

        var existingTodoList = userService.GetById(id);
        if (existingTodoList is null)
        {
            return NotFound();
        }
        userService.Update(newUser.Id, newUser);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]

    public IActionResult Delete(int id)
    {
        var User = userService.GetById(id);
        if (User is null)
            return NotFound();
        userService.Delete(id);
        todolistService.DeleteByUserId(id);
        return NoContent();
    }
}

