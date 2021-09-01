using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebHost.DomainModels.Responses
{
    public class GetUserInfoResponse
    {
        public string CarNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
    }
}