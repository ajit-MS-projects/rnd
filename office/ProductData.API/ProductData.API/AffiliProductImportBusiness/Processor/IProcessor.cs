using System;
using Affilinet.Business.ProductImport.Entity;

namespace Affilinet.Business.ProductImport.Processor
{
    /// <summary>
    /// Defines common interface for a CSV processor class
    /// </summary>
    public interface IProcessor : IDisposable
    {
        /// <summary>
        /// Gets or sets the doc attrib list, this is main object for all product, program and field settings.
        /// One must refer to this object for any settings or information
        /// </summary>
        /// <value>The doc attrib list.</value>
        DocumentAttributes DocAttribs { get; set; }
        /// <summary>
        /// This method initiate the processing of source CSV/XMl documemt.
        /// </summary>
        void ProcessDocumemt();
        /// <summary>
        /// Create a delimiter seperated header line for processed CSV files.
        /// </summary>
        void PrepareHeader();
        /// <summary>
        /// Writes to CSV files in chunks.
        /// </summary>
        void WriteToCSVFiles();
        /// <summary>
        /// Finishes the writing CSV files.
        /// Writes CSV for deleted products
        /// </summary>
        void FinishWritingCSVFiles();

        /// <summary>
        /// Writes to product files.
        /// </summary>
        void WriteToProductFiles();
        /// <summary>
        /// Writes to deleted product file.
        /// </summary>
        void WriteToDeletedProductFile();

        /// <summary>
        /// This is a function to create the property XML that is later stored in the Properties column of
        /// the Product Table.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="value">The value.</param>
        /// <param name="isNumeric">if set to <c>true</c> [is numeric].</param>
        /// <param name="objProduct">The obj product.</param>
        /// <returns></returns>
        string CreatePropertyNode(string title, string value, bool isNumeric, Product objProduct);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        void Dispose(bool disposing);
    }
}