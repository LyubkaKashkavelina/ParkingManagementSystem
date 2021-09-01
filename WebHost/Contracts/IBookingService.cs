namespace WebHost.Services
{
    using System.Collections.Generic;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public interface IBookingService 
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "AllFreeParkingSpaces",
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<DomainModels.Responses.GetAllFreeParkingSpacesResponse> GetAllFreeParkingSpaces();

        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "AllFreeParkingSpacesByUser",
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<DomainModels.Responses.GetAllFreeParkingSpacesByUserIdResponse> GetAllFreeParkingSpacesByUser();

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "CancelFreeParkingSpace",
            ResponseFormat = WebMessageFormat.Json)]
        HttpStatusCode CancelFreeParkingSpace(DomainModels.Requests.CancelFreeParkingSpace FreeParkingSpace);

        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "AllBookingsByUser",
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<DomainModels.Responses.GetAllBookingsByUserResponse> GetAllBookingsByUser();

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "BookFreeParkingSpace",
            ResponseFormat = WebMessageFormat.Json)]
        DomainModels.Responses.BookFreeParkingSpaceResponse BookFreeParkingSpace(DomainModels.Requests.BookFreeParkingSpaceRequest booking);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "CancelBooking",
            ResponseFormat = WebMessageFormat.Json)]
        HttpStatusCode CancelBooking(DomainModels.Requests.CancelBookingRequest booking);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SetFreeParkingSpace",
            ResponseFormat = WebMessageFormat.Json)]
        DomainModels.Responses.SetParkingSpaceAsFreeResponse SetParkingSpaceAsFree(DomainModels.Requests.SetParkingSpaceAsFreeRequest freeParkingSpaceModel);
    }
}
