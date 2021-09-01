using System;
using System.Runtime.Serialization;
using System.Web;

namespace WebHost.DomainModels.Requests
{
    public class BookFreeParkingSpaceRequest : DateSerizalizer
    {
        public Guid ParkingSpaceId { get; set; }
    }
}