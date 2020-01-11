using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace WcfServices
{
    [ServiceContract]
    public interface ICrossDomain
    {
        [OperationContract]
        [WebGet(UriTemplate = "clientaccesspolicy.xml")]
        Message ProvidePolicyFile();

    }
}