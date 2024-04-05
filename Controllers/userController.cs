using Microsoft.AspNetCore.Mvc;
using todoList.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using todoList.Services;
using todoList.Models;
namespace myTodoLists.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService, ITodolistService todolistService) : ControllerBase
{
    readonly ITodolistService todolistService = todolistService;
    readonly IUserService userService = userService;
    [HttpPost]
    [Route("/login")]
    //Login by name and password, returns token
    public ActionResult<String> Login(string name, string password)
    {
        User? user = userService.Authentication(name, password);
        if (user != default)
        {
            var claims = new List<Claim>
            {
                new Claim("type", user.Status.ToString()),
                new Claim("id", user.Id.ToString()),
            };
            var token = TokenService.GetToken(claims);
            return new OkObjectResult(TokenService.WriteToken(token));
        }
        return Unauthorized();
    }

    [HttpGet]
    [Route("/allUsers")]
    [Authorize(Policy = "Admin")]
    //Get all users.
    public ActionResult<List<todoList.Models.User>> GetAll() =>
            userService.GetAll();

    [HttpGet]
    [Route("/myUser")]
    [Authorize(Policy = "User")]
    //Get user by id.
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
    //Add new user.
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
    //Update user.
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
    //Delete user.
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

