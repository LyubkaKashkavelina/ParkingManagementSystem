namespace WebHost.DomainModels.Responses
{
    using System;

    public class BookFreeParkingSpaceResponse : DateSerizalizer
    {
        public Guid ParkingSpaceId { get; set; }
    }
}