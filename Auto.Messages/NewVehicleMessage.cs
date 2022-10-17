using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Messages
{
    public class NewVehicleMessage
    {
        public string Registration { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelCode { get; set; }
        public string ModelName { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        public DateTime ListedAtUtc { get; set; }
    }
}
