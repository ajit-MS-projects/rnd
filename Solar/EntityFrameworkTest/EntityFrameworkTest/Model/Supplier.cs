using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkTest
{
    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        [Column("ID")]
        public string SupplierCode { get; set; }
        
        [Required]
        [StringLengthAttribute(200)]
        public string Name { get; set; }

        //public virtual ICollection<Product> Products { get; set; }
    }
}
