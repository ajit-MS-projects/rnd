using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

namespace CastleActiveRecordTest
{

	public static class PersistenceHelper
	{
		public static void Initialize()
		{
			IConfigurationSource source = ConfigurationManager.GetSection("activerecord") as IConfigurationSource;
			var assembly = Assembly.Load("CastleActiveRecordTest");
			if (!ActiveRecordStarter.IsInitialized)
			{
				ActiveRecordStarter.Initialize(assembly, source);
				ActiveRecordStarter.CreateSchema();
			}
		}
		public static void CreateDbSchema()
		{
			if (ActiveRecordStarter.IsInitialized)
				ActiveRecordStarter.CreateSchema();
		}
	} 
}