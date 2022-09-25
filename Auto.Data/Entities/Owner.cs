using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auto.Data.Entities
{
    public class Owner
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string VehicleId { get; set; }

        [JsonIgnore]
        public virtual Vehicle Vehicle { get; set; }
    }
}
