using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;

namespace NihibernateTest
{
	public static class PersistenceHelper
	{
		public static void Initialize()
		{
			IConfigurationSource source = ConfigurationManager.GetSection("activerecord") as IConfigurationSource;

			var assembly = Assembly.Load("NihibernateTest");


			if (!ActiveRecordStarter.IsInitialized)
			{
			    ActiveRecordStarter.Initialize(assembly, source);
			}
		}
	}
}