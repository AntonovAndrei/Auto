using System.Text.Json.Serialization;

namespace Auto.Core.Entities;

public class Vehicle
{
    public string Registration { get; set; }
    public string ModelCode { get; set; }
    public string Color { get; set; }
    public int Year { get; set; }

    [JsonIgnore]
    public virtual Model VehicleModel { get; set; }
}