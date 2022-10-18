using Auto.Data.Entities;
using Auto.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace Auto.Website.Extensions;

public static class HypermediaExtensions
{
    public static dynamic ToDynamic(this object value)
    {
        IDictionary<string, object> expando = new ExpandoObject();
        var properties = TypeDescriptor.GetProperties(value.GetType());
        foreach (PropertyDescriptor property in properties)
        {
            if (Ignore(property)) continue;
            expando.Add(property.Name, property.GetValue(value));
        }
        return (ExpandoObject)expando;
    }

    private static bool Ignore(PropertyDescriptor property)
    {
        if (property.Name == "LazyLoader") return true;
        return property.Attributes.OfType<Newtonsoft.Json.JsonIgnoreAttribute>().Any();
    }

    public static dynamic Paginate(string url, int index, int count, int total)
    {
        dynamic links = new ExpandoObject();
        links.self = new { href = url };
        links.final = new { href = $"{url}?index={total - total % count}&count={count}" };
        links.first = new { href = $"{url}?index=0&count={count}" };
        if (index > 0) links.previous = new { href = $"{url}?index={index - count}&count={count}" };
        if (index + count < total) links.next = new { href = $"{url}?index={index + count}&count={count}" };
        return links;
    }

    public static NewVehicleMessage ToMessage(this Vehicle vehicle)
    {
        var message = new NewVehicleMessage()
        {
            Registration = vehicle.Registration,
            ManufacturerName = vehicle.VehicleModel?.Manufacturer?.Name,
            ModelName = vehicle.VehicleModel?.Name,
            ModelCode = vehicle.VehicleModel?.Code,
            Color = vehicle.Color,
            Year = vehicle.Year,
            ListedAtUtc = DateTime.UtcNow
        };

        return message;
    }
    
    public static NewOwnerMessage ToMessage(this Owner owner)
    {
        var message = new NewOwnerMessage()
        {
            Id = owner.Id,
            FullName = owner.FullName,
            BirthDate = owner.BirthDate,
            ListedAtUtc = DateTime.UtcNow
        };

        return message;
    }
}