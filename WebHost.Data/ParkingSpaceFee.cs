//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebHost.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ParkingSpaceFee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParkingSpaceFee()
        {
            this.ParkingSpaces = new HashSet<ParkingSpace>();
        }
    
        public System.Guid ParkingSpaceFeeId { get; set; }
        public string Category { get; set; }
        public decimal MonthlyFee { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParkingSpace> ParkingSpaces { get; set; }
    }
}
