using System;
using System.Collections.Generic;

#nullable disable

namespace ParkingAdministration.Data
{
    public partial class ParkingSpace
    {
        public ParkingSpace()
        {
            UserToParkingSpaces = new HashSet<UserToParkingSpace>();
        }

        public Guid ParkingSpaceId { get; set; }
        public Guid ParkingSpaceFeeId { get; set; }
        public string ParkingSpaceNumber { get; set; }

        public virtual ParkingSpaceFee ParkingSpaceFee { get; set; }
        public virtual ICollection<UserToParkingSpace> UserToParkingSpaces { get; set; }
    }
}
