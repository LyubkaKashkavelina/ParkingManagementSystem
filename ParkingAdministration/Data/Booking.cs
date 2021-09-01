using System;
using System.Collections.Generic;

#nullable disable

namespace ParkingAdministration.Data
{
    public partial class Booking
    {

        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public Guid ParkingSpaceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MonthlyFee { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual FreeParkingSpace ParkingSpace { get; set; }
        public virtual User User { get; set; }
    }
}
