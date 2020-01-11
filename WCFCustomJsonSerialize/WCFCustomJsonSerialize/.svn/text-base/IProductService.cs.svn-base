using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using Newtonsoft.Json;

namespace WCFCustomJsonSerialize
{
    [ServiceContract]
    public interface IProductServiceXml
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Xml,
           ResponseFormat = WebMessageFormat.Xml,
           UriTemplate = "SearchProducts?query={query}")]
        ProductWebResponse SearchProducts(String query);
    }
    [ServiceContract]
    public interface IProductServiceJson
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "SearchProducts?query={query}")]
        ProductWebResponse SearchProducts(String query);
    }
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProductService" in both code and config file together.
    [ServiceContract]
    public interface IProductService
    {
        [OperationContract]
        ProductResponse SearchProducts(ProductRequest request);
    }

    [DataContract]
    //[Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public class Product
    {
        [DataMember]
        [JsonProperty]
        public int ID { get; set; }
        [DataMember]
        [JsonProperty]
        public String Description { get; set; }
        [DataMember]
        [JsonProperty]
        public decimal Price { get; set; }
        [DataMember]
        [JsonProperty]
        public String Title { get; set; }
        [DataMember, Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]//, Newtonsoft.Json.JsonProperty]
        public ProductTypeEnum ProductType { get; set; }
        //[DataMember]
        //public String ProductType
        //{
        //    get { return ProductType1.ToString(); }
        //    private set { }
        //}
        [DataMember]
        [JsonProperty]
        public String ProductTypeS
        {
            get { return ProductType.ToString(); }
            private set { }
        }
    }

    [MessageContract]
    public class ProductRequest
    {
        [MessageBodyMember]
        public String Query { get; set; }
    }
    [MessageContract]
    public class ProductResponse
    {
        [MessageBodyMember]
        public List<Product> Products { get; set; }
    }
    public class ProductWebResponse
    {
        public List<Product> Products { get; set; }
    }
    public enum ProductTypeEnum
    {
        [EnumMember(Value = "ConsumerGood")] 
        ConsumerGood,
        [EnumMember(Value = "Hotel")] 
        Hotel,
        [EnumMember(Value = "Travel")] 
        Travel
    }
}
