using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WebHost.Services
{
    [ServiceContract]
    public interface IPaymentService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "Payments",
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<DomainModels.Payment> GetAllPayments();

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Pay",
            ResponseFormat = WebMessageFormat.Json)]
        DomainModels.Payment Pay(DomainModels.Payment payment);
    }
}
