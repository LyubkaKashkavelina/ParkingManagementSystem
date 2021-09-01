namespace WebHost.DomainModels.Responses
{
    using System;

    public class GetAllFreeParkingSpacesByUserIdResponse : DateSerizalizer
    {
        public bool IsBooked { get; set; }

        public Guid FreeParkingSpaceId { get; set; }
    }
}