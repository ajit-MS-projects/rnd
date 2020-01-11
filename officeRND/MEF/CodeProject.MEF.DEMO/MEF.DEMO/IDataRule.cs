using System.Xml.Linq;

namespace MEF.DEMO
{
    public interface IDataRule
    {
        bool IsValid(XDocument document);
    }
}