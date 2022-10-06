using System.Text.Json.Serialization;

namespace Auto.Core.Entities;

public class Model
{
    public Model() {
        Vehicles = new HashSet<Vehicle>();
    }

    public string Code { get; set; }
    public string ManufacturerCode { get; set; }
    public string Name { get; set; }

    public virtual Manufacturer Manufacturer { get; set; }
    [JsonIgnore]
    public virtual ICollection<Vehicle> Vehicles { get; set; }
}