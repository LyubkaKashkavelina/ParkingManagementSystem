namespace WebHost.Services
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using WebHost.DomainModels.Requests;
    using WebHost.Ioc;
    using WebHost.Repository.Repositories;
    using WebHost.Repository.RepositoryContracts;
    using WebHost.Security;

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly Guid userId;
        public BookingService()
        {
            this._bookingRepository = IocManager.RegisterType<BookingRepository, IBookingRepository>();
            this._mapper = IocManager.RegisterMapper();

            var tokenRecogniser = new TokenRecogniser<Data.Booking>(this._bookingRepository);
            this.userId = tokenRecogniser.DecodeToken();
        }

        public IEnumerable<DomainModels.Responses.GetAllFreeParkingSpacesResponse> GetAllFreeParkingSpaces()
        {
            var allFreeParkingSpaces = this._bookingRepository.GetAllFreeParkingSpaces(this.userId);

            var mappedFreeParkingSpaces = this._mapper.Map<IEnumerable<Data.GetAllFreeParkingSpaces_Result>, IEnumerable<DomainModels.Responses.GetAllFreeParkingSpacesResponse>>(allFreeParkingSpaces);

            return mappedFreeParkingSpaces;
        }

        public IEnumerable<DomainModels.Responses.GetAllFreeParkingSpacesByUserIdResponse> GetAllFreeParkingSpacesByUser()
        {
            var allFreeParkingSpaces = this._bookingRepository.GetAllFreeParkingSpacesByUser(this.userId);

            var mappedFreeParkingSpaces = this._mapper.Map<IEnumerable<Data.GetAllFreeParkingSpacesByUserId_Result>, IEnumerable<DomainModels.Responses.GetAllFreeParkingSpacesByUserIdResponse>>(allFreeParkingSpaces);

            return mappedFreeParkingSpaces;
        }

        public IEnumerable<DomainModels.Responses.GetAllBookingsByUserResponse> GetAllBookingsByUser()
        {
            var allBookings = this._bookingRepository.GetAllBookingsByUser(this.userId);

            var mappedBookings = this._mapper.Map<IEnumerable<Data.GetAllBookingsByUser_Result>, IEnumerable<DomainModels.Responses.GetAllBookingsByUserResponse>>(allBookings);

            return mappedBookings;
        }

        public DomainModels.Responses.BookFreeParkingSpaceResponse BookFreeParkingSpace(DomainModels.Requests.BookFreeParkingSpaceRequest booking)
        {
            try
            {
                if (booking == null)
                {
                    throw new ArgumentNullException($"Can't book parking space with id {booking.ParkingSpaceId}.");
                }

                var bookingToBeSet = this._mapper.Map<DomainModels.Requests.BookFreeParkingSpaceRequest, Data.Booking>(booking);

                var newBooking = this._bookingRepository.BookFreeParkingSpace(bookingToBeSet, this.userId);

                return this._mapper.Map<Data.Booking, DomainModels.Responses.BookFreeParkingSpaceResponse>(newBooking);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        public DomainModels.Responses.SetParkingSpaceAsFreeResponse SetParkingSpaceAsFree(DomainModels.Requests.SetParkingSpaceAsFreeRequest parkingSpace)
        {
            try
            {
                if (parkingSpace == null)
                {
                    throw new ArgumentNullException($"Can't set parking space with id {parkingSpace} as free.");
                }

                var freeParkingSpace = this._mapper.Map<DomainModels.Requests.SetParkingSpaceAsFreeRequest, Data.FreeParkingSpace>(parkingSpace);

                var newFreeParkingSpace = this._bookingRepository.SetParkingSpaceAsFree(freeParkingSpace, this.userId);

                return this._mapper.Map<Data.FreeParkingSpace, DomainModels.Responses.SetParkingSpaceAsFreeResponse>(newFreeParkingSpace);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        public HttpStatusCode CancelFreeParkingSpace(CancelFreeParkingSpace freeParkingSpace)
        {
            try
            {
                if(freeParkingSpace == null)
                {
                    throw new ArgumentNullException("Can't cancel Free Parking Space.");
                }
                return this._bookingRepository.CancelFreeParkingSpace(freeParkingSpace.FreeParkingSpaceId);
            }
            catch(Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        public HttpStatusCode CancelBooking(CancelBookingRequest booking)
        {
            try
            {
                if (booking == null)
                {
                    throw new ArgumentNullException("Can't cancel Free Parking Space.");
                }
                return this._bookingRepository.CancelBooking(booking.BookingId);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}
