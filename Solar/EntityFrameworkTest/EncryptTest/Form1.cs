using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EntityFrameworkTest;

namespace EncryptTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            using (var db = new ProductContext())
            {
                String category = "L";
                CheckAndAddCategory(db, category, "Lego");
                CheckAndAddAddress(db, new Address() { AddressId = 2, Country = "DE", Street = "johann clanze str 55" });

                var product = new Product() { Name = "Raumschiff", Test = "nicht wichtig", CategoryId = "L", AddressId = 2, Price = 100.36m };
                db.Products.Add(product);

                int recordsAffected = db.SaveChanges();

                //Assert.IsTrue(recordsAffected > 0);
            }
        }

        private static void CheckAndAddCategory(ProductContext db, string categoryId, string categoryName)
        {
            // check, if category exists
            var categoryList = db.Categories.Find(categoryId);

            if (categoryList == null) // add, if no
            {
                Category objCategory = new Category() { CategoryId = categoryId, Name = categoryName };
                db.Categories.Add(objCategory);
            }
        }

        private static void CheckAndAddAddress(ProductContext db, Address address)
        {
            // check, if category exists
            var addressList = db.Addresses.Find(address.AddressId);

            if (addressList == null) // add, if no
            {
                Address objAddress = new Address() { AddressId = address.AddressId, Street = address.Street, Country = address.Country };
                db.Addresses.Add(objAddress);
            }
        }

        private void btnEncryptConfig_Click(object sender, EventArgs e)
        {
            EncryptConfig();
        }
        private void EncryptConfig()
        {
            Configuration config;
            ConfigurationSection configSection;
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
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
