namespace WebHost.Services
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using WebHost.DomainModels;

    [ServiceContract]
    public interface IUserService
    {

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Register",
            ResponseFormat = WebMessageFormat.Json)]
        DomainModels.User Register(DomainModels.AuthModel newUser);


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Login",
            ResponseFormat = WebMessageFormat.Json)]
        string Login(DomainModels.AuthModel loginModel);

        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "AllUsers",
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<DomainModels.User> GetAllUsers();

        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "CurrentUser",
            ResponseFormat = WebMessageFormat.Json)]
        DomainModels.CurrentUser GetCurrentUser();

        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "UserInfo",
            ResponseFormat = WebMessageFormat.Json)]
        DomainModels.Responses.GetUserInfoResponse GetCarInfo();

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SetUserInfo",
            ResponseFormat = WebMessageFormat.Json)]
        DomainModels.Responses.SetUserInfoResponse SetCarInfo(DomainModels.Requests.SetUserInfoRequest carInfo);
    }
}
