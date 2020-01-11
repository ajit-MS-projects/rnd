using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Business.ProductImport.Entity
{
    public class ProductHash
    {
        public string HashCode { get; set; }
        public string InsertUpdateDelete { get; set; }
        public string ProductId { get; set; }
        public int ImageOk { get; set; }
        public string UpdateDate { get; set; }
        public ProductHash(string hashCode, string insertUpdateDelete, string productId, int imageOk, string updateDate)
        {
            this.HashCode = hashCode;
            this.InsertUpdateDelete = insertUpdateDelete;
            this.ProductId = productId;
            this.ImageOk = imageOk;
            this.UpdateDate = updateDate;
        }
    }
}
