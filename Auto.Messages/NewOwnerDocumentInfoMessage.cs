﻿namespace Auto.Messages;

public class NewOwnerMatriculaMessage: NewOwnerMessage
{
    public int VehicleCount { get; set; }

    public NewOwnerMatriculaMessage(NewOwnerMessage message,
        int vehicleCount)
    {
        FullName = message.FullName;
        BirthDate = message.BirthDate;
        VehicleCount = vehicleCount;
    }
}