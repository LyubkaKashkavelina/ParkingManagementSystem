namespace WebHost.DomainModels.Responses
{
    using System;

    public class SetParkingSpaceAsFreeResponse : DateSerizalizer
    {
        public Guid FreeParkingSpaceId { get; set; }

        public string ParkingSpaceNumber { get; set; }
    }
}