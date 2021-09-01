namespace WebHost.Repository.RepositoryContracts
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using WebHost.Data;

    public interface IBookingRepository : IRepository<Booking>
    {
        IEnumerable<GetAllFreeParkingSpaces_Result> GetAllFreeParkingSpaces(Guid userId);

        IEnumerable<GetAllFreeParkingSpacesByUserId_Result> GetAllFreeParkingSpacesByUser(Guid userId);

        IEnumerable<GetAllBookingsByUser_Result> GetAllBookingsByUser(Guid userId);

        Data.Booking BookFreeParkingSpace(Booking booking, Guid userId);

        string FindParkingSpaceNumber(Booking booking);

        Data.FreeParkingSpace SetParkingSpaceAsFree(FreeParkingSpace freeParkingSpace, Guid userId);

        HttpStatusCode CancelFreeParkingSpace(Guid freeParkingSpaceId);

        HttpStatusCode CancelBooking(Guid bookingId);

    }
}
