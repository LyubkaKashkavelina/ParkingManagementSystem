namespace WebHost.Mapping
{
    using AutoMapper;
    using System.Collections.Generic;
    using WebHost.Data;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Resource
            CreateMap<Data.User, DomainModels.User>();
            CreateMap<Data.Booking, DomainModels.Responses.BookFreeParkingSpaceResponse>();
            CreateMap<Data.Payment, DomainModels.Payment>();
            CreateMap<Data.User, DomainModels.AuthModel>();
            CreateMap<Data.User, DomainModels.AuthModel>();
            CreateMap<Data.User, DomainModels.CurrentUser>();
            CreateMap<Data.Booking, DomainModels.Responses.BookFreeParkingSpaceResponse>();
            CreateMap<Data.Car, DomainModels.Responses.GetUserInfoResponse>();
            CreateMap<Data.Car, DomainModels.Responses.SetUserInfoResponse>();

            CreateMap<Data.FreeParkingSpace, DomainModels.Responses.SetParkingSpaceAsFreeResponse>();

            CreateMap<Data.GetAllFreeParkingSpacesByUserId_Result, DomainModels.UserFreeSpace>();
            CreateMap<IEnumerable<Data.GetAllFreeParkingSpacesByUserId_Result>, List<DomainModels.UserFreeSpace>>();
            CreateMap<Data.GetAllBookingsByUser_Result, DomainModels.Responses.GetAllBookingsByUserResponse>();
            CreateMap<IEnumerable<Data.GetAllBookingsByUser_Result>, List<DomainModels.Responses.GetAllBookingsByUserResponse>>();
            CreateMap<Data.GetAllFreeParkingSpacesByUserId_Result, DomainModels.Responses.GetAllFreeParkingSpacesByUserIdResponse>();
            CreateMap<IEnumerable<Data.GetAllFreeParkingSpacesByUserId_Result>, List<DomainModels.Responses.GetAllFreeParkingSpacesByUserIdResponse>>();
            CreateMap<Data.GetAllFreeParkingSpaces_Result, DomainModels.Responses.GetAllFreeParkingSpacesResponse>();
            CreateMap<IEnumerable<Data.GetAllFreeParkingSpaces_Result>, List<DomainModels.Responses.GetAllFreeParkingSpacesResponse>>();


            // Resource to Domain
            CreateMap<DomainModels.User, Data.User>();
            CreateMap<DomainModels.Requests.BookFreeParkingSpaceRequest, Data.Booking>();
            CreateMap<DomainModels.Payment, Data.Payment>();
            CreateMap<DomainModels.AuthModel, Data.User>();
            CreateMap<DomainModels.AuthModel, Data.User>();
            CreateMap<DomainModels.CurrentUser, Data.User>();

            CreateMap<DomainModels.Requests.SetParkingSpaceAsFreeRequest, Data.FreeParkingSpace>();
            CreateMap<DomainModels.Requests.SetUserInfoRequest, Data.Car>();

            CreateMap<DomainModels.Responses.BookFreeParkingSpaceResponse, Data.Booking>();
            CreateMap<DomainModels.UserFreeSpace, Data.GetAllFreeParkingSpacesByUserId_Result>();
            CreateMap<IEnumerable<DomainModels.UserFreeSpace>, List<Data.GetAllFreeParkingSpacesByUserId_Result>>();
            CreateMap<DomainModels.Requests.BookFreeParkingSpaceRequest, Data.GetAllBookingsByUser_Result>();
            CreateMap<IEnumerable<DomainModels.Requests.BookFreeParkingSpaceRequest>, List<Data.GetAllBookingsByUser_Result>>();
            CreateMap<DomainModels.Responses.GetAllFreeParkingSpacesByUserIdResponse, Data.GetAllFreeParkingSpacesByUserId_Result>();
            CreateMap<IEnumerable<DomainModels.Responses.GetAllFreeParkingSpacesByUserIdResponse>, List<Data.GetAllFreeParkingSpacesByUserId_Result>>();
            CreateMap<DomainModels.Responses.GetAllFreeParkingSpacesResponse, Data.GetAllFreeParkingSpaces_Result>();
            CreateMap<IEnumerable<DomainModels.Responses.GetAllFreeParkingSpacesResponse>, List<Data.GetAllFreeParkingSpaces_Result>>();
        }
    }
}