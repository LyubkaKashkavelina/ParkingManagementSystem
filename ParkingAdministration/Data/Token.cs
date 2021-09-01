using System;
using System.Collections.Generic;

#nullable disable

namespace ParkingAdministration.Data
{
    public partial class Token
    {
        public Guid TokenId { get; set; }
        public string UserToken { get; set; }
    }
}
