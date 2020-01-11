using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkTest
{
    public class Product : BasisObjekt
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public decimal Price { get; set; }
    //        public ICollection<Supplier> Suppliers { get; set; }

        //public virtual ICollection<Supplier> Suppliers { get; set; }
        public int AddressId { get; set; }


        //public virtual Supplier Supplier { get; set; }
        public virtual Address Address { get; set; }
        public virtual Category Category { get; set; }

        [NotMappedAttribute]
        public string Test { get; set; }
    }
}
