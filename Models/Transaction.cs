namespace lesson2.Models;
public class Transaction(int tranId, int senderId, int receiverId, double transactionAmount)
{
    public int TranId { get; set;} = tranId;
    public int SenderId { get; set;} = senderId;
    public int ReceiverId { get; set;} = receiverId;
    public double TransactionAmount { get; set;} = transactionAmount;

}