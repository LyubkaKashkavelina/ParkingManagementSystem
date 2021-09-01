using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHost.DomainModels.Responses
{
    public class SetUserInfoResponse
    {
        public string CarNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string PhoneNumber { get; set; }
    }
}