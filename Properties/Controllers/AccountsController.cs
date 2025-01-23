using lesson2.Models;
using Microsoft.AspNetCore.Mvc;

namespace lesson2.Properties.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountsController : ControllerBase

{
    public static List<Account> _accounts = new List<Account>();
    
    [HttpPost("create")]
    public IActionResult CreateAccount(string name, int userId)
    {
        var user = UsersController._users.FirstOrDefault(user => user.Id == userId);
        if (user == null) 
        {
            return NotFound($"Юзер с id {userId} не найден");
        }
        var lastAcc = _accounts.LastOrDefault(account => account.UserId == userId);
        var lastAccId = lastAcc != null ? lastAcc.SimpleId : 0;
        _accounts.Add(new Account(++lastAccId, name, userId, 0.0));
        user.HasAccount = true;
        return Ok();
    }
    [HttpGet]
    public IActionResult GetAccounts()
    {
        return Ok(_accounts);
    }
    [HttpGet("byId")]
    public IActionResult GetAccountsById(int userId)
    {
        User? user = UsersController._users.FirstOrDefault(user => user.Id == userId);
        if (user == null) 
        {
            return NotFound($"Юзер с id {userId} не найден");
        }
        if (user.HasAccount == false) 
        {
            return NotFound($"У юзера с id {userId} нет открытых счетов");
        }
        List<Account> accountsById = _accounts.FindAll(account => account.UserId == userId);
        return Ok(accountsById);
    }
    [HttpPost("replenish")]
    public IActionResult Replenish(int userId, int simpleId, double amount)
    {
        Account? accountToReplenish = _accounts.FirstOrDefault(account => userId == account.UserId && simpleId == account.SimpleId);
        if (accountToReplenish == null) 
        {
            return NotFound($"У юзера с id {userId} не найден счет с simpleId {simpleId}");
        }
        accountToReplenish.Balance = accountToReplenish.Balance + amount;
        return Ok($"Аккаунт успешно пополнен, текущий баланс: {accountToReplenish.Balance}");
    }
    [HttpPost("withdrawal")]
    public IActionResult Withdrawal(int userId, int simpleId, double amount)
    {
        Account? accountToWithdraw = _accounts.FirstOrDefault(account => userId == account.UserId && simpleId == account.SimpleId);
        if (accountToWithdraw == null) 
        {
            return NotFound($"У юзера с id {userId} не найден счет с simpleId {simpleId}");
        }
        if (accountToWithdraw.Balance < amount) 
        {
            return BadRequest($"Недостаточно денег для снятия, введите сумму не более {accountToWithdraw.Balance}");
        }
        accountToWithdraw.Balance = accountToWithdraw.Balance - amount;
        return Ok($"Снятие прошло успешно, текущий баланс: {accountToWithdraw.Balance}");
    }
}