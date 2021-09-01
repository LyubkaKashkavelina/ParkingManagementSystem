using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ParkingAdministration.Data
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Cars = new HashSet<Car>();
            Payments = new HashSet<Payment>();
            UserToParkingSpaces = new HashSet<UserToParkingSpace>();
        }

        public Guid UserId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Employee field is required.")]
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? CreationDate { get; set; }
        //[Required(ErrorMessage = "Phone field is required.")]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<UserToParkingSpace> UserToParkingSpaces { get; set; }
    }
}
