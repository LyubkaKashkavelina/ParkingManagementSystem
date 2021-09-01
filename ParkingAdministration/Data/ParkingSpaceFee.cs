using System;
using System.Collections.Generic;

#nullable disable

namespace ParkingAdministration.Data
{
    public partial class ParkingSpaceFee
    {
        public ParkingSpaceFee()
        {
            ParkingSpaces = new HashSet<ParkingSpace>();
        }

        public Guid ParkingSpaceFeeId { get; set; }
        public string Category { get; set; }
        public decimal MonthlyFee { get; set; }

        public virtual ICollection<ParkingSpace> ParkingSpaces { get; set; }
    }
}
