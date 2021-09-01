namespace WebHost.DomainModels.Requests
{
    using System;
    using System.Runtime.Serialization;
    using System.Web;

    public class CancelBookingRequest
    {
        public Guid BookingId { get; set; }
    }
}
