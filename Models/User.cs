namespace lesson2.Models;

public class User(string name, int id, double balance)
{
    public string Name { get; set; } = name;
    public int Id { get; set; } = id;
    public double Balance { get; set;} = balance;
}