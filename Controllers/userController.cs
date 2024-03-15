using Microsoft.AspNetCore.Mvc;
using todoList.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using todoList.Services;
using todoList.Models;
namespace myTodoLists.Controllers;

[ApiController]
[Route("/user")]
public class UserController(IUserService userService, ITodolistService todolistService) : ControllerBase
{
    readonly ITodolistService todolistService = todolistService;
    readonly IUserService userService = userService;
    [HttpPost]
    [Route("/login")]
    public ActionResult<String> Login(int id, string password)
    {
        System.Console.WriteLine(id);
        System.Console.WriteLine(password);
        if (id == 1 && password == "bbb")
        {
            System.Console.WriteLine(id);
            var claims = new List<Claim>
            {
                new Claim("type", "Admin"),
                new Claim("id","1"),
            };
            var token = TokenService.GetToken(claims);
            return new OkObjectResult(TokenService.WriteToken(token));
        }
        if (userService.Authentication(id, password))
        {
            var claims = new List<Claim>
            {
                new Claim("type", "User"),
                new Claim("id", id.ToString()),
            };
            var token = TokenService.GetToken(claims);
            return new OkObjectResult(TokenService.WriteToken(token));
        }
        return Unauthorized();
    }

    [HttpGet]
    [Route("/allUser")]
    [Authorize(Policy = "Admin")]
    public ActionResult<List<todoList.Models.User>> GetAll() =>
            userService.GetAll();


    [HttpGet]
    [Route("/myUser")]
    [Authorize(Policy = "User")]
    public ActionResult<todoList.Models.User> GetById()
    {
        var userID = User.FindFirst("id")?.Value;
        if (userID is null)
            return NotFound();
        var myUser = userService.GetById(Convert.ToInt32(userID));
        if (myUser == null)
            return NotFound();
        return myUser;
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public IActionResult Create(todoList.Models.User newUser)
    {
        if (newUser is null)
        {
            return BadRequest("User object is null");
        }
        userService.Add(newUser);
        return CreatedAtAction(nameof(Create), new { id = newUser.Id }, newUser);
    }

    [HttpPut]
    [Authorize(Policy = "User")]
    public IActionResult Update(User userToUpdate)
    {
        if (userToUpdate is null)
        {
            return BadRequest("User object is null");
        }
        var userID = User.FindFirst("id")?.Value;
        if (userID is null)
            return NotFound();
        int id = Convert.ToInt32(userID);
        User? user = userService.GetById(id);
        if (user is null)
        {
            return NotFound("User not found");
        }
        user.Username = userToUpdate.Username ?? user.Username;
        user.Password = userToUpdate.Password ?? user.Password;
        userService.Update(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public IActionResult Delete(int id)
    {
        var user = userService.GetById(id);
        if (user is null)
            return NotFound();
        userService.Delete(id);
        todolistService.DeleteByUserId(id);
        return NoContent();
    }
}

