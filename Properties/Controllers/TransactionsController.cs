using lesson2.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace lesson2.Properties.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController : ControllerBase
{
    public static List<Transaction> _transactions = new List<Transaction>();
    static int lastTranId = 0;
    [HttpPost]
    public IActionResult Transact(int senderId, int receiverId, int senderSimpleId, int receiverSimpleId, double transactionAmount)
    {
        User? sender = UsersController._users.FirstOrDefault(user => user.Id == senderId);
        if (sender == null) 
        {
            return NotFound($"Юзер с id {senderId} не найден");
        }
        Account? accountSender = AccountsController._accounts.FirstOrDefault(accountSender => accountSender.SimpleId == senderSimpleId);
        if (accountSender == null)
        {
            return NotFound($"Счет отправителя с SimpleId {senderSimpleId} не найден");
        }
        User? receiver = UsersController._users.FirstOrDefault(user => user.Id == receiverId);
        if (receiver == null) 
        {
            return NotFound($"Юзер с id {receiverId} не найден");
        } 
        Account? accountReceiver = AccountsController._accounts.FirstOrDefault(accountReceiver => accountReceiver.SimpleId == receiverSimpleId);
        if (accountReceiver == null)
        {
            return NotFound($"Счет получателя с SimpleId {accountReceiver} не найден");
        }
        if 
        (receiverId == senderId && receiverSimpleId == senderSimpleId) 
        {
            return BadRequest("Счета отправителя и получателя совпадают");
            //Account? account = AccountsController._accounts.FirstOrDefault(account => account.SimpleId == receiverSimpleId && receiverSimpleId != senderSimpleId);
            // проверка на наличие >1 счета 
            // + между разными юзерами
        }
        
        /*
        if (transactionAmount > sender.Balance) 
        {
            return BadRequest("На счете недостаточно средств!");
        }
        
        sender.Balance = sender.Balance - transactionAmount;
        receiver.Balance = receiver.Balance + transactionAmount;
        _transactions.Add(new Transaction(++lastTranId, senderId, receiverId, transactionAmount)); */
        return Ok($"Транзакция на сумму {transactionAmount} прошла успешно!");
    }

    [HttpGet]
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