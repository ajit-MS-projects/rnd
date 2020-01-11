using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIMaintenanceTasks
{
    /// <summary>
    /// Wrapper class to wrap all constant structures
    /// </summary>
    public class MConstants
    {
        /// <summary>
        /// Stored procedures which only return a data set
        /// </summary>
        public struct ReadOnlyStoredProcs
        {
            public const string MnGetImagesForHashing = "MnGetImagesForHashing";
            public const string CreateProductImageTemp = "MnCreateProductImageTemp";
            public const string UpdateImagesHashing = "MnUpdateImagesHashing";
            public const string MnGetCountOfImagesForHashing = "MnGetCountOfImagesForHashing";
        }

        public struct Generic
        {
            public const string ProductImageTable = " ProductImage";
        }
        public struct ProductImage
        {
            public const string ImageID = "ID";
            public const string ProductProgramID = "ProductProgramID";
            public const string ImageURL = "ImageURL";
            public const string HashCode = "HashCode";
        }
    }
}
