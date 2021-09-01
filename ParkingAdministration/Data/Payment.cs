using System;
using System.Collections.Generic;

#nullable disable

namespace ParkingAdministration.Data
{
    public partial class Payment
    {
        public Guid PaymentId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal PaidAmount { get; set; }

        public virtual User User { get; set; }
    }
}
