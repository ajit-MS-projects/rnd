using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfSession
{
	public class Service1 : IService1
	{
		
		private int i = 0;
		public string GetData()
		{
			i++;
			string s = string.Format("Session id: {0} and i={1}", OperationContext.Current.SessionId,i);

			return s;
		}
	}
}
