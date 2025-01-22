using lesson2.Models;
using Microsoft.AspNetCore.Mvc;

namespace lesson2.Properties.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private static List<User> _users = new List<User>();
    private static List<Transaction> _transactions = new List<Transaction>();
    static int lastId = 0;
    static int lastTranId = 0;

    [HttpPost]
    public IActionResult AddUser(string name, double balance)
    {
        _users.Add(new User(name, ++lastId, balance));
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

    [HttpPost]
    public IActionResult Transact(int senderId, int receiverId, double transactionAmount)
    {
        User? sender = _users.FirstOrDefault(user => user.Id == senderId);
        if (sender == null) 
        {
            return NotFound($"Юзер с id {senderId} не найден");
        }
        User? receiver = _users.FirstOrDefault(user => user.Id == receiverId);
        if (receiver == null) 
        {
            return NotFound($"Юзер с id {receiverId} не найден");
        }
        if (transactionAmount > sender.Balance) 
        {
            return BadRequest("На счете недостаточно средств!");
        }
        sender.Balance = sender.Balance - transactionAmount;
        receiver.Balance = receiver.Balance + transactionAmount;
        _transactions.Add(new Transaction(++lastTranId, senderId, receiverId, transactionAmount));
        return Ok($"Транзакция на сумму {transactionAmount} прошла успешно!");
    }

    [HttpGet]
    public IActionResult PrintTransactionsById(int id)
    {
        User? userToFind = _users.FirstOrDefault(user => user.Id == id);
        if (userToFind == null)
        {
            return NotFound($"Юзер с id {id} не найден");
        }
        List<Transaction> userTransactions = _transactions.FindAll(transaction => id == transaction.SenderId || id == transaction.ReceiverId);
        return Ok(userTransactions);
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