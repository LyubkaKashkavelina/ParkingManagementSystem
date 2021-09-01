using ParkingAdministration.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAdministration.Models
{
    public class UserToParkingSpaceEntity
    {
        public UserToParkingSpace UserToParkingSpace { get; set; }
        public Car Vehicle { get; set; }
        //[Required]
        public string PhoneNumber { get; set; }
    }
}
