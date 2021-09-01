using System;
using System.Collections.Generic;

#nullable disable

namespace ParkingAdministration.Data
{
    public partial class FreeParkingSpace
    {
        public FreeParkingSpace()
        {
            Bookings = new HashSet<Booking>();
        }

        public Guid FreeParkingSpaceId { get; set; }
        public Guid UserSpaceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual UserToParkingSpace UserSpace { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
