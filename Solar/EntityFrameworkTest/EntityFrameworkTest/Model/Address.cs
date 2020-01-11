using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkTest
{
    [Table("Addresses")]
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
    }
}