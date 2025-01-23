using lesson2.Models;
using Microsoft.AspNetCore.Mvc;

namespace lesson2.Properties.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    public static List<User> _users = new List<User>();
    static int lastId = 0;
    [HttpPost]
    public IActionResult AddUser(string name)
    {
        _users.Add(new User(name, ++lastId, false));
        return Ok();
    }
    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(_users);
    }
    [HttpDelete]
    public IActionResult DeleteUser(int id)
    {
        int index = _users.FindIndex(user => user.Id == id);
        if (index == -1)
        {
            return NotFound($"Юзер с id {id} не найден");
        }
        _users.RemoveAt(index);
        return Ok();
    }
    
    /*
     добавить transact и print_transactions by id
     
     со звездочкой:
     есть список счетов
     у одного юзера может быть несколько счетов
     он может открывать счета и переводить деньги между своими и другим
     добавить метод пополнить replenish 
     
     мегазвездочка:
     мультивалютные счета
     учитывать курс при переводе
     */
}