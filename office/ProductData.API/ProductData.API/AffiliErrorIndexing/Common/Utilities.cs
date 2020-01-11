using System;
using System.Collections.Generic;
using System.Linq;

namespace AffiliErrorIndexing.Common
{
    /// <summary>
    /// Contains common utility methods
    /// </summary>
    public class Utilities
    {
        private static Dictionary<string, string> _weightageConfigToReportLog = null;
        private static List<string> ErrorIndicatorWithoutCalculation = null;
        
        /// <summary>
        /// Computes the deviation of a sequence of system.double values
        /// </summary>
        /// <param name="num"></param>
        /// <returns>The Standard Deviation value</returns>
        internal static double StandardDeviation(List<double> num)
        {
            double sumOfSqrs = 0;
            double avg = num.Average();
            for (int i = 0; i < num.Count; i++)
            {
                sumOfSqrs += Math.Pow(((double)num[i] - avg), 2);
            }
            double n = (double)num.Count;
            return Math.Sqrt(sumOfSqrs / (n - 1));
        }

        /// <summary>
        /// Calculate the top and bottom deviation values
        /// </summary>
        /// <param name="num"></param>
        /// <param name="deviationFactor"></param>
        /// <param name="deviation"></param>
        /// <param name="deviationTop"></param>
        /// <param name="deviationBottom"></param>
        /// <returns>The top and the bottom deviation values (range).</returns>
        /// <remarks>
        /// Einfache Standardabweichung: x = x ± 1*s    Die einfache Standardabweichung erfasst 68 % aller Stichprobenwerte
        /// Doppelte Standardabweichung: x = x ± 2*s    Die doppelte Standardabweichung erfasst 95,5 % aller Stichprobenwerte.
        /// Dreifache Standardabweichung:x = x ± 3*s    Die dreifache Standardabweichung erfasst 99,7 % aller Stichprobenwerte.
        /// </remarks>
        internal static void CalculateDeviationRange(List<double> num, int deviationFactor, double deviation, out double deviationTop, out double deviationBottom)
        {
            deviationTop = num.Average() + deviationFactor * deviation;
            deviationBottom = num.Average() - deviationFactor * deviation;
            if (deviationBottom < 0)    // bottom should not be negative (BusinessRule PM)
                deviationBottom = 0;
        }

        /// <summary>
        /// Check if a value is greater than deviationTop or smaller than deviationBottom
        /// </summary>
        /// <param name="lastCycleValue"></param>
        /// <param name="deviationTop"></param>
        /// <param name="deviationBottom"></param>
        /// <returns>True if value is outside the deviationTop and deviationBottom parameter</returns>
        internal static bool LastValueIsOutsideRange(double lastCycleValue, double deviationTop, double deviationBottom)
        {
            return (lastCycleValue < deviationBottom || lastCycleValue > deviationTop);
        }

        /// <summary>
        /// Mapps the entries from Pa_WeightageConfiguration table to the entries from the ReportLog table to get the data from ReportLog table for the ErrorIndex calculation
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string, string > GetMappedErrorIndicator()
        {
            if (_weightageConfigToReportLog == null)
            {
                _weightageConfigToReportLog = new Dictionary<string, string>();
            // this list only contains the indicators which have to be calculated! Not the HeaderError, DownloadError...
            //Pa_WeightageConfiguration --> ReportLog
            _weightageConfigToReportLog.Add(Constants.ErrorIndexMasterListNames.TotalProducts, Constants.MasterListNames.TotalProductsProcessed);
            //_weightageConfigToReportLog.Add(Constants.ErrorIndexMasterListNames.TotalImages, Constants.MasterListNames.TotalImagesProcessed);   // TODO: write this number into report log
            //_weightageConfigToReportLog.Add(Constants.ErrorIndexMasterListNames.ProductsWithInvalidImageUrl, Constants.MasterListNames.ProductsWithInvalidImageUrl);    // TODO: write this number into report log
            _weightageConfigToReportLog.Add(Constants.ErrorIndexMasterListNames.SanitationTimeError, Constants.MasterListNames.SanitizationDuration); 
            _weightageConfigToReportLog.Add(Constants.ErrorIndexMasterListNames.DownloadTimeError, Constants.MasterListNames.DownloadDuration);   
            _weightageConfigToReportLog.Add(Constants.ErrorIndexMasterListNames.UpdatedProducts, Constants.MasterListNames.UpdatedProducts);
            _weightageConfigToReportLog.Add(Constants.ErrorIndexMasterListNames.MappedProducts, Constants.MasterListNames.ProductsInAffiliCategories);
            _weightageConfigToReportLog.Add(Constants.ErrorIndexMasterListNames.NewShopCategories, Constants.MasterListNames.NewProductCategories);
            _weightageConfigToReportLog.Add(Constants.ErrorIndexMasterListNames.TotalShopCategories, Constants.MasterListNames.TotalShopCategories);
            _weightageConfigToReportLog.Add(Constants.ErrorIndexMasterListNames.ProductsWithoutCategoryText, Constants.MasterListNames.ProductsWithoutCategoryText);
            _weightageConfigToReportLog.Add(Constants.ErrorIndexMasterListNames.ProductsWithInvalidDeeplinkUrl, Constants.MasterListNames.ProductsWithInvalidDeeplinkUrl);
            }
            return _weightageConfigToReportLog;
        }

        /// <summary>
        /// Return these indicators where no calculation is required; 
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetErrorIndicatorWithoutCalculation()
        {
            if (ErrorIndicatorWithoutCalculation == null)
            {
                ErrorIndicatorWithoutCalculation = new List<string>();
                ErrorIndicatorWithoutCalculation.Add(Constants.ErrorIndexMasterListNames.DownloadError);
                ErrorIndicatorWithoutCalculation.Add(Constants.ErrorIndexMasterListNames.HeaderError);
                ErrorIndicatorWithoutCalculation.Add(Constants.ErrorIndexMasterListNames.OtherSanitisationError);
            }
            return ErrorIndicatorWithoutCalculation;
        }
    }
}
