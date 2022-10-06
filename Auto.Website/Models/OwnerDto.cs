using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Website.Models
{
    public class OwnerDto
    {
        [DisplayName("Owner Id")]
        public int Id { get; set; }
        [Required]
        [DisplayName("Owner name, surname, patronymic")]
        public string FullName { get; set; }
        [Required]
        [DisplayName("Birthdate")]
        public DateTime BirthDate { get; set; }
        
        [DisplayName("Vehicle code")]
        public string VehicleId { get; set; }
    }
}
