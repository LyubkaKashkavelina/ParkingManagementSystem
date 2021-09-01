using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ParkingAdministration.Data
{
    public partial class UserToParkingSpace
    {
        public UserToParkingSpace()
        {
            FreeParkingSpaces = new HashSet<FreeParkingSpace>();
        }

        public Guid UserToParkingSpaceId { get; set; }
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Parking space field is required.")]
        public Guid ParkingSpaceId { get; set; }
        [DataType(DataType.Date), Required(ErrorMessage = "Start date is required.")]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public virtual ParkingSpace ParkingSpace { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<FreeParkingSpace> FreeParkingSpaces { get; set; }
    }
}
