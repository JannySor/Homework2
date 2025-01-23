using lesson2.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace lesson2.Properties.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController : ControllerBase
{
    private static List<Transaction> _transactions = new List<Transaction>();
    [HttpPost("create")]
    public IActionResult Transact(int senderId, int receiverId, int senderSimpleId, int receiverSimpleId, double transactionAmount)
    {
        User? sender = UsersController._users.FirstOrDefault(user => user.Id == senderId);
        if (sender == null) 
        {
            return NotFound($"Юзер с id {senderId} не найден");
        }
        Account? accountSender = AccountsController._accounts.FirstOrDefault(accountSender => accountSender.SimpleId == senderSimpleId && accountSender.UserId == senderId);
        if (accountSender == null)
        {
            return NotFound("Счет отправителя не найден");
        }
        User? receiver = UsersController._users.FirstOrDefault(user => user.Id == receiverId);
        if (receiver == null) 
        {
            return NotFound($"Юзер с id {receiverId} не найден");
        } 
        Account? accountReceiver = AccountsController._accounts.FirstOrDefault(accountReceiver => accountReceiver.SimpleId == receiverSimpleId && accountReceiver.UserId == receiverId);
        if (accountReceiver == null)
        {
            return NotFound("Счет получателя не найден");
        }
        if 
        (receiverId == senderId && receiverSimpleId == senderSimpleId) 
        {
            return BadRequest("Счета отправителя и получателя совпадают");
        }
        if (transactionAmount > accountSender.Balance) 
        {
            return BadRequest("На счете недостаточно средств!");
        }
        var lastTransaction = _transactions.LastOrDefault();
        var lastTranId = lastTransaction != null ? lastTransaction.TranId : 0;
        _transactions.Add(new Transaction(++lastTranId, senderId, receiverId, transactionAmount));
        accountSender.Balance = accountSender.Balance - transactionAmount;
        accountReceiver.Balance = accountReceiver.Balance + transactionAmount;
        return Ok($"Транзакция на сумму {transactionAmount} прошла успешно!");
    }
    [HttpGet]
    public IActionResult PrintTransactions()
    {
        return Ok(_transactions);
    }
    [HttpGet("byId")]
    public IActionResult PrintTransactionsById(int id)
    {
        User? userToFind = UsersController._users.FirstOrDefault(user => user.Id == id);
        if (userToFind == null)
        {
            return NotFound($"Юзер с id {id} не найден");
        }
        List<Transaction> userTransactions = _transactions.FindAll(transaction => id == transaction.SenderId || id == transaction.ReceiverId);
        return Ok(userTransactions);
    }
}