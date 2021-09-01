using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ParkingAdministration.Data
{
    public partial class Car
    {
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
        //[Required(ErrorMessage = "License Plate field is required.")]
        public string CarNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        public virtual User User { get; set; }
    }
}
