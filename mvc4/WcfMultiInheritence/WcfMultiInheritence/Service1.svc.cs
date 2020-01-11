using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfMultiInheritence
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
        public Gebaeude GetDataUsingDataContract(DachkonstruktionParameter p_DachkonstruktionParameterNeu)
        {
            Gebaeude gebaeude = new Gebaeude();
            gebaeude.ID = 1234;

            return gebaeude;
        }

        public CompositeType GetDataUsingDataContract1(CompositeType p_DachkonstruktionParameterNeu)
        {
            return new CompositeType(){StringValue = p_DachkonstruktionParameterNeu.StringValue};
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
