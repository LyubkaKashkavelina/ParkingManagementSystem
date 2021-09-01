namespace WebHost.Repository.Validation
{
    using System;

    public static class BookingRepositoryValidator
    {
        public static void SetParkingSpaceAsFreeValidator(Data.FreeParkingSpace freeParkingSpace, Guid userId)
        {
            if (freeParkingSpace == null || userId == null)
            {
                throw new ArgumentNullException($"Can't set parking space '{freeParkingSpace.FreeParkingSpaceId}' parking space for user with id '{userId}'");
            }

            if (freeParkingSpace.StartDate.Date < DateTime.Now.Date || freeParkingSpace.EndDate < freeParkingSpace.StartDate)
            {
                throw new InvalidOperationException("Trying to set parking space as free for invalid dates.");
            }
        }

        public static void BookFreeParkingSpaceValidator(Data.Booking booking, Data.FreeParkingSpace bookedFreeParkingSpace)
        {
            if (booking.StartDate<bookedFreeParkingSpace.StartDate || booking.EndDate> bookedFreeParkingSpace.EndDate)
            {
                throw new InvalidOperationException("Booking dates are not valid.");
            }
        }
    }
}
