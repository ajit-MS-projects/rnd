using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Affilinet.Business.ProductExport.Common;
using Constants = Affilinet.Business.ProductExport.Common.Constants;

namespace Affilinet.Business.ProductExport.Entity
{
    public class ProgramFilesList : IDisposable
    {
        private List<String> _FileNames = null;
        public String ProdProgId { get; set; }
        public String ExportTimeStamp { get; set; }
        public String ModifiedDateTime { get; set; }

        public List<String> FileNames
        {
            get
            {
                if (_FileNames == null)
                {
                    String[] fileExtensions = Utilities.GetFileExtensions();
                    String csvFilePath = Utilities.GetAppSettingValue(Constants.AppSettings.CsvFilesPath);
                    CheckIfFileExists();

                    _FileNames = new List<String>();
                    foreach (String fileExtension in fileExtensions) //Return all files of a program
                    {
                        int i = 1;

                        String fileName = ExportTimeStamp + "_" + ProdProgId + "_" + i.ToString() + fileExtension;
                        fileName = csvFilePath + ProdProgId + @"\" + fileName;
                        while (File.Exists(fileName))
                        {
                            _FileNames.Add(fileName); //20090104_103204_29_1.product.update.csv
                            i++;
                            fileName = ExportTimeStamp + "_" + ProdProgId + "_" + i.ToString() + fileExtension;
                            fileName = Utilities.GetAppSettingValue(Constants.AppSettings.CsvFilesPath) + ProdProgId + @"\" + fileName;
                        }
                    }
                }
                return _FileNames;
            }
        }

        internal void CheckIfFileExists()
        {
            String csvFilePath = Utilities.GetAppSettingValue(Constants.AppSettings.CsvFilesPath);
            string filePath = Path.Combine(csvFilePath, ProdProgId);
            if (!Directory.Exists(filePath))
                return;

            DirectoryInfo sourceDir = new DirectoryInfo(filePath);
            FileInfo[] existingFiles = sourceDir.GetFiles(ExportTimeStamp + "*");
            // check if minimum one file with the given timestamp exist
            if (ExportTimeStamp !="" && existingFiles.GetLength(0) != 0)
                return;

            // if no file exist get the newest file from this folder 
            if (sourceDir.GetFiles("*" + Constants.ExportFileExtensions.CSV).Length > 0)
            {
                FileInfo myFile = sourceDir.GetFiles("*" + Constants.ExportFileExtensions.CSV).OrderByDescending(f => f.LastWriteTime).First();

                string[] splitTimeStamp = myFile.Name.Split(new char[] {'_'});

                if (splitTimeStamp.Length > 1)
                    ExportTimeStamp = splitTimeStamp[0] + "_" + splitTimeStamp[1];
                if (ModifiedDateTime=="")
                    ModifiedDateTime = myFile.LastWriteTime.ToString();
            }
            
        }

            #region Dispose
        public void CleanProduct()
        {
            PropertyInfo[] arrPi = this.GetType().GetProperties();
            foreach (PropertyInfo pi in arrPi)
            {
                if (pi.CanWrite)
                    pi.SetValue(this, null, null);
            }
        }
        private bool disposed;
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!(this.disposed))
            {
                if (disposing)
                {
                    CleanProduct();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// It also suppresses finalization
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);//This call is to makes sure that disposed object does not get put on the GC's finalize queue.
        }
        /// <summary>
        /// Finalizes this instance, if dispose is not called explicitly
        /// </summary>
        protected void Finalize()
        {
            Dispose(false);
        }
        #endregion
    }
}
