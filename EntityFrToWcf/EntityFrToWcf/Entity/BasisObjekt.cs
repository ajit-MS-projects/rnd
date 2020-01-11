using System.Runtime.Serialization;

namespace EntityFrToWcf
{
    public interface IBasisObjekt
    {
        //[DataMember]
        int Id { get; set; }
    }

    [DataContract]
    public abstract class BasisObjekt : IBasisObjekt
    {
        [DataMember]
        public int Id { get; set; }
    }
}