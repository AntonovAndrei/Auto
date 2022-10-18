namespace Auto.Messages;

public class NewOwnerMessage
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime ListedAtUtc { get; set; }
}