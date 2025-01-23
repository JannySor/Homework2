namespace lesson2.Models;

public class User(string name, int id, bool hasAccount)
{
    public string Name { get; set; } = name;
    public int Id { get; set; } = id;
    public bool HasAccount { get; set;} = hasAccount;
}