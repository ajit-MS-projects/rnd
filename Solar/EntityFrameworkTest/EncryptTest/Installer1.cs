using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;


namespace EncryptTest
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            EncryptConfig();
        }
        private void EncryptConfig()
        {
            Configuration config;
            ConfigurationSection configSection;
            config = ConfigurationManager.OpenExeConfiguration(Context.Parameters["assemblypath"]); 

            configSection = config.ConnectionStrings;
            if (configSection != null)
            {
                if (!(configSection.SectionInformation.IsLocked))
                {
                    configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    config.Save();

                }
            }
        }
    }
}
