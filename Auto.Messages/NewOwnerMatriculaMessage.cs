namespace Auto.Messages;

public class NewOwnerMatriculaMessage: NewOwnerMessage
{
    public int VehicleCount { get; set; }

    public NewOwnerMatriculaMessage(){}
    public NewOwnerMatriculaMessage(NewOwnerMessage message,
        int vehicleCount)
    {
        FullName = message.FullName;
        BirthDate = message.BirthDate;
        VehicleCount = vehicleCount;
    }

    public NewOwnerMatriculaMessage(string fullName,
        DateTime birthDate, int vehicleCount)
    {
        FullName = fullName;
        BirthDate = birthDate;
        VehicleCount = vehicleCount;
    }
}