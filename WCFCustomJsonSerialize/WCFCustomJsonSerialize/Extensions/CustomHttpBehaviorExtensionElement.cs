using System;
using JsonNetMessageFormatter;


namespace WCFCustomJsonSerialize.Extensions
{
    class CustomHttpBehaviorExtensionElement : System.ServiceModel.Configuration.BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(NewtonsoftJsonBehavior); }
        }

        private bool useFaultException = false;


        
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            if (name == "faultExceptionEnabled")
            {
                this.useFaultException = value == "true" ? true : false;
                return true;
            }
            return false;
        }

        protected override object CreateBehavior()
        {
            object result = new NewtonsoftJsonBehavior();
            ((NewtonsoftJsonBehavior)result).FaultExceptionEnabled = useFaultException;
            return result;
        }
    }
}
