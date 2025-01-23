public class Account(int simpleId, string name, int userId, double balance)
{
    public int SimpleId { get; set;} = simpleId;
    public string Name { get; set;} = name;
    public int UserId { get; set;} = userId;
    public double Balance { get; set;} = balance;
}