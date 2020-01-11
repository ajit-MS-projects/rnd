using System.Reflection;

namespace EntityFrameworkTest
{
    public class BasisObjekt
    {
        public void CopyObject(object to)
        {
           
            PropertyInfo[] fromPropertyInfos = this.GetType().GetProperties();
            foreach (PropertyInfo frPropInfo in fromPropertyInfos)
            {
                object fromValue = frPropInfo.GetValue(this, null);//get the value of property from from FROM object
                PropertyInfo toPropInfo = to.GetType().GetProperty(frPropInfo.Name);//get the property with same from TO object
                if (toPropInfo!=null) toPropInfo.SetValue(to, fromValue, null);//if TO object has this property set the value
            }
        }

        //public void CopyObject_notWorking(object to)
        //{
        //    //if (to == null)
        //    //    to = System.Activator.CreateInstance(to.GetType());
        //    //if(from.GetType() != to.GetType() )
        //    //{
        //    //    throw new Exception(
        //    //        "Error in BasisObjekt.CopyObject(), type of from and to object must be same in order to copy them. from is of type: " +
        //    //        from.GetType() + ", whereas to is of type: " + to.GetType());
        //    //}

        //    //PropertyInfo[] fromPropertyInfos = this.GetType().GetProperties();
        //    //foreach (PropertyInfo frPropInfo in fromPropertyInfos)
        //    //{
        //    //    object fromValue = frPropInfo.GetValue(this, null);//get the value of property from from FROM object
        //    //    Type toType = to.GetType();


        //    //    PropertyInfo toPropInfo = toType.GetProperty(frPropInfo.Name);//get the property with same from TO object
        //    //    if (toPropInfo != null)
        //    //    {
        //    //        MethodInfo solarObjectMethodInfo = toType.GetMethod("CopyObject");
        //    //        if (solarObjectMethodInfo != null)
        //    //            solarObjectMethodInfo.Invoke();
        //    //        else
        //    //            toPropInfo.SetValue(to, fromValue, null); //if TO object has this property set the value
        //    //    }
        //    //}
        //}
    }
}