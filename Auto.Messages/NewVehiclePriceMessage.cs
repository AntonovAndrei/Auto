using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Messages
{
    public class NewVehiclePriceMessage : NewVehicleMessage
    {
        public int Price { get; set; }
        public string CurrencyCode { get; set; }

        public NewVehiclePriceMessage(NewVehicleMessage message, 
            int price, string currencyCode)
        {
            Registration = message.Registration;
            Color = message.Color;
            Year = message.Year;
            ManufacturerName = message.ManufacturerName;
            ModelName = message.ModelName;
            Price = price;
            CurrencyCode = currencyCode;
        }
    }
}
