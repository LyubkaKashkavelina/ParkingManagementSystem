namespace WebHost.Repository.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.ServiceModel.Web;
    using WebHost.Data;
    using WebHost.Repository.RepositoryContracts;
    using WebHost.Repository.Validation;

    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(ParkingManagementSystemEntities context)
            : base(context)
        { }

        private ParkingManagementSystemEntities ParkingManagementSystemContext
        {
            get 
            { 
                return Context as ParkingManagementSystemEntities; 
            }
        }

        public IEnumerable<GetAllFreeParkingSpaces_Result> GetAllFreeParkingSpaces(Guid userId)
        {
            var allFreeParkingSpaces = this.ParkingManagementSystemContext.GetAllFreeParkingSpaces().Where(x => DateTime.Compare(x.EndDate, DateTime.Today) >= 0);
            var freeParkingSpacesFromOtherUsers = allFreeParkingSpaces.Where(x => x.ParkingSpaceNumber != this.ParkingManagementSystemContext.GetUserParkingSpaceNumber(userId).FirstOrDefault());
            return freeParkingSpacesFromOtherUsers;
        }

        public IEnumerable<GetAllFreeParkingSpacesByUserId_Result> GetAllFreeParkingSpacesByUser(Guid userId)
        {
            var allFreeParkingSpacesByUser = this.ParkingManagementSystemContext.GetAllFreeParkingSpacesByUserId(userId).ToList();
            var allFreeParkingSpacesInTheFuture = allFreeParkingSpacesByUser.Where(x => x.EndDate.Date >= DateTime.Now.Date || x.IsBooked == true);
            return allFreeParkingSpacesInTheFuture;
        }

        public IEnumerable<GetAllBookingsByUser_Result> GetAllBookingsByUser(Guid userId)
            => this.ParkingManagementSystemContext.GetAllBookingsByUser(userId).ToList();

        public string FindParkingSpaceNumber(Booking booking)
            => this.ParkingManagementSystemContext.GetBookingsParkingSpaceNumber(booking.ParkingSpaceId).FirstOrDefault();

        public Data.Booking BookFreeParkingSpace(Data.Booking booking, Guid userId)
        {
            var isAlreadyBooked = this.ParkingManagementSystemContext.Bookings.Where(x => x.ParkingSpaceId == booking.ParkingSpaceId).FirstOrDefault();
            if(isAlreadyBooked != null)
            {
                throw new ArgumentException("This place has already been booked.");
            }

            var bookedFreeParkingSpace = this.ParkingManagementSystemContext.FreeParkingSpaces
                .Where(x => x.FreeParkingSpaceId == booking.ParkingSpaceId)
                .FirstOrDefault();

            BookingRepositoryValidator.BookFreeParkingSpaceValidator(booking, bookedFreeParkingSpace);

            var fee = this.ParkingManagementSystemContext
                .GetFreeParkingSpaceFeeById(booking.ParkingSpaceId)
                .FirstOrDefault();

            booking.BookingId = Guid.NewGuid();
            booking.UserId = userId;
            booking.ParkingSpaceId = booking.ParkingSpaceId;
            booking.StartDate = booking.StartDate.ToLocalTime().Date;
            booking.EndDate = booking.EndDate.ToLocalTime().Date;
            booking.MonthlyFee = (decimal)fee;
            booking.CreationDate = DateTime.Now;

            this.ParkingManagementSystemContext.Bookings.Add(booking);
            this.ParkingManagementSystemContext.SaveChanges();

            this.BookingResolver(booking, bookedFreeParkingSpace);

            return booking;
        }

        public Data.FreeParkingSpace SetParkingSpaceAsFree(Data.FreeParkingSpace freeParkingSpace, Guid userId)
        {
            BookingRepositoryValidator.SetParkingSpaceAsFreeValidator(freeParkingSpace, userId);

            var userToParkingSpace = this.ParkingManagementSystemContext.UserToParkingSpaces
                .Where(x => x.UserId == userId && x.StartDate <= freeParkingSpace.StartDate && (x.EndDate >= freeParkingSpace.EndDate || x.EndDate == null))
                .FirstOrDefault<UserToParkingSpace>();

            if(userToParkingSpace == null)
            {
                throw new ArgumentException("The parking space is not assigned to the current user for this period.");
            }

            var freeParkingSpacesForLoggedUser = this.ParkingManagementSystemContext.FreeParkingSpaces.Where(x => x.UserSpaceId == userToParkingSpace.UserToParkingSpaceId);
            if(freeParkingSpacesForLoggedUser.Where(x => (x.StartDate >= freeParkingSpace.StartDate  && x.EndDate <= freeParkingSpace.EndDate) || 
                                                        (freeParkingSpace.StartDate >= x.StartDate && freeParkingSpace.StartDate <= x.EndDate) || 
                                                        (freeParkingSpace.EndDate >= x.StartDate && freeParkingSpace.EndDate <= x.EndDate)).FirstOrDefault() != null)
            {
                throw new ArgumentException("You have already set your parking space free for this period");
            }

            freeParkingSpace.FreeParkingSpaceId = Guid.NewGuid();
            freeParkingSpace.UserSpaceId = userToParkingSpace.UserToParkingSpaceId;
            freeParkingSpace.StartDate = freeParkingSpace.StartDate.ToLocalTime();
            freeParkingSpace.EndDate = freeParkingSpace.EndDate.ToLocalTime();
            freeParkingSpace.CreationDate = DateTime.Now;

            this.ParkingManagementSystemContext.FreeParkingSpaces.Add(freeParkingSpace);
            this.ParkingManagementSystemContext.SaveChanges();

            return freeParkingSpace;
        }

        /// <summary>
        /// This method splits the freeParkingSpace record if needed.There are three cases:
        ///     - If a user books this place for the whole period, the freeParkingSpace is not split and no updates are made
        ///     - If a user books this place for a part of the whole period starting from freeParkingSpace.StartDate, the freeParkingSpace.EndDate is changed to the value of 
        ///       booking.EndDate and a new record for FreeParkingSpace object is inserted into database.
        ///     - If a user books this place for a part of the whole period finishing at freeParkingSpace.EndDate, the freeParkingSpace.StartDate is changed to the value of 
        ///       booking.StartDate and a new record for FreeParkingSpace object is inserted into database.
        ///     - If a user books this place for a part of the whole period between freeParkingSpace.StartDate and freeParkingSpace.EndDate, 
        ///       the freeParkingSpace.StartDate is changed to the value of booking.StartDate and the freeParkingSpace.EndDate is changed to the value of booking.EndDate 
        ///       and two new records for FreeParkingSpace objects are inserted into database.
        /// </summary>
        /// <param name="booking"></param>
        /// <param name="freeParkingSpace"></param>
        private void BookingResolver(Data.Booking booking, Data.FreeParkingSpace freeParkingSpace)
        {
            var freeParkingSpaceRecord = this.ParkingManagementSystemContext.FreeParkingSpaces
                .Where(x => x.FreeParkingSpaceId == freeParkingSpace.FreeParkingSpaceId)
                .FirstOrDefault(); 

            if(booking.StartDate == freeParkingSpace.StartDate && booking.EndDate != freeParkingSpace.EndDate)
            { 
                var newStartDate = booking.EndDate.AddDays(1);

                var newFreeParkingSpaceRecord = new Data.FreeParkingSpace
                {
                    FreeParkingSpaceId = Guid.NewGuid(),
                    UserSpaceId = freeParkingSpace.UserSpaceId,
                    StartDate = newStartDate,
                    EndDate = freeParkingSpace.EndDate,
                    CreationDate = freeParkingSpace.CreationDate
                };

                freeParkingSpaceRecord.EndDate = booking.EndDate;

                this.ParkingManagementSystemContext.FreeParkingSpaces.Add(newFreeParkingSpaceRecord);
                this.ParkingManagementSystemContext.SaveChanges();
            }
            else if(booking.StartDate != freeParkingSpace.StartDate && booking.EndDate == freeParkingSpace.EndDate)
            {
                var newEndDate = booking.StartDate.AddDays(-1);

                var newFreeParkingSpaceRecord = new Data.FreeParkingSpace
                {
                    FreeParkingSpaceId = Guid.NewGuid(),
                    UserSpaceId = freeParkingSpace.UserSpaceId,
                    StartDate = freeParkingSpace.StartDate,
                    EndDate = newEndDate,
                    CreationDate = freeParkingSpace.CreationDate
                };

                freeParkingSpaceRecord.StartDate = booking.StartDate;

                this.ParkingManagementSystemContext.FreeParkingSpaces.Add(newFreeParkingSpaceRecord);
                this.ParkingManagementSystemContext.SaveChanges();
            }
            else if(booking.StartDate != freeParkingSpace.StartDate && booking.EndDate != freeParkingSpace.EndDate)
            {           
                var firstNewEndDate = booking.StartDate.AddDays(-1);

                var FirstNewFreeParkingSpaceRecord = new Data.FreeParkingSpace
                {
                    FreeParkingSpaceId = Guid.NewGuid(),
                    UserSpaceId = freeParkingSpace.UserSpaceId,
                    StartDate = freeParkingSpace.StartDate,
                    EndDate = firstNewEndDate,
                    CreationDate = freeParkingSpace.CreationDate
                };

                var secondNewStartDate = booking.EndDate.AddDays(1);

                var SecondNewFreeParkingSpaceRecord = new Data.FreeParkingSpace
                {
                    FreeParkingSpaceId = Guid.NewGuid(),
                    UserSpaceId = freeParkingSpace.UserSpaceId,
                    StartDate = secondNewStartDate,
                    EndDate = freeParkingSpace.EndDate,
                    CreationDate = freeParkingSpace.CreationDate
                };

                freeParkingSpaceRecord.StartDate = booking.StartDate;
                freeParkingSpaceRecord.EndDate = booking.EndDate;

                this.ParkingManagementSystemContext.FreeParkingSpaces.Add(FirstNewFreeParkingSpaceRecord);
                this.ParkingManagementSystemContext.FreeParkingSpaces.Add(SecondNewFreeParkingSpaceRecord);
                this.ParkingManagementSystemContext.SaveChanges();
            }
        }

        public HttpStatusCode CancelFreeParkingSpace(Guid freeParkingSpaceId)
        {
            var freeParkingSpace = this.ParkingManagementSystemContext.FreeParkingSpaces.Where(x => x.FreeParkingSpaceId == freeParkingSpaceId).FirstOrDefault();
            if(freeParkingSpace == null)
            {
                throw new ArgumentNullException("The FreeParkingSpace with the given Id is not found.");
            }
            this.ParkingManagementSystemContext.FreeParkingSpaces.Remove(freeParkingSpace);
            this.ParkingManagementSystemContext.SaveChanges();
            return HttpStatusCode.OK;
        }

        public HttpStatusCode CancelBooking(Guid bookingId)
        {
            var booking = this.ParkingManagementSystemContext.Bookings.Where(x => x.BookingId == bookingId).FirstOrDefault();
            if (booking == null)
            {
                throw new ArgumentNullException("The Booking with the given Id is not found.");
            }
            this.ParkingManagementSystemContext.Bookings.Remove(booking);
            this.ParkingManagementSystemContext.SaveChanges();
            return HttpStatusCode.OK;
        }

    }
}
