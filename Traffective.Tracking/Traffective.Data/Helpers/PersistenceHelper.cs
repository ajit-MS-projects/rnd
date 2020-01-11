using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;

namespace Traffective.Data.Helpers
{
    public static class PersistenceHelper
    {
        /// <summary>
        /// Initializes the active record with all db entities instance.
        /// Default: lazy loading is enabled
        /// </summary>
        public static void Initialize()
        {
            IConfigurationSource source = ConfigurationManager.GetSection("activerecord") as IConfigurationSource;
            var assembly = Assembly.Load("Traffective.Data");
            if (!ActiveRecordStarter.IsInitialized)
            {
                ActiveRecordStarter.Initialize(assembly, source);
            }
        }
        /// <summary>
        /// Creates the db schema (code first approach).
        /// </summary>
        public static void CreateDbSchema()
        {
            if (ActiveRecordStarter.IsInitialized)
                ActiveRecordStarter.CreateSchema();
        }
    }
}