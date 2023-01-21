namespace Auto.Messages;

public class NewOwnerDocumentInfoMessage: NewOwnerMessage
{
    public int DrivingExperienceInMonth { get; set; }
    public DateTime DocumentReceiptDate { get; set; }

    public NewOwnerDocumentInfoMessage(NewOwnerMessage message,
        int experienceInMonth, DateTime documentReceiptDate)
    {
        FullName = message.FullName;
        BirthDate = message.BirthDate;
        DrivingExperienceInMonth = experienceInMonth;
        DocumentReceiptDate = documentReceiptDate;
    }
}