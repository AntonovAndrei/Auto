namespace Auto.Messages;

public class NewOwnerMessage
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime ListedAtUtc { get; set; }

    public NewOwnerMessage()
    {
        var rnd = new Random();
        Id = rnd.Next(1, Int32.MaxValue);
        ListedAtUtc = DateTime.UtcNow;
    }
}