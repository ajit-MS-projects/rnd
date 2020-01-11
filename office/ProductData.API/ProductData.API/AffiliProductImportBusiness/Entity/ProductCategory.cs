using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Business.ProductImport.Entity
{
    public class ProductCategory
    {
        /// <summary>
        /// Gets or sets the shop category id.
        /// </summary>
        /// <value>The shop category id.</value>
        public string ShopCategoryId { get; set; }
        /// <summary>
        /// Gets or sets the affili category id.
        /// </summary>
        /// <value>The affili category id.</value>
        public string AffiliCategoryId { get; set; }
        /// <summary>
        /// Gets or sets the cat path text, shop category seperator is replaced by affili category seperator here.
        /// </summary>
        /// <value>The cat path text.</value>
        public string CatPathText { get; set; }
    }
}
