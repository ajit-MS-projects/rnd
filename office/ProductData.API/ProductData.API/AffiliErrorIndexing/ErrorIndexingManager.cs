using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AffiliErrorIndexing.DAO;
using Affilinet.Exceptions;
using CommonUtilities = Affili.ProductData.Common.Utilities;
using AffiliErrorIndexing.Common;

namespace AffiliErrorIndexing
{
    /// <summary>
    /// Error index calculation (QualityAlerting)
    /// </summary>
    public class ErrorIndexingManager
    {
        #region Private Instances
        private ErrorIndexingDAO _dao;
        private Dictionary<string, string> _dicErrIndicators;
        private List<string> _listErrIndicatorsWithoutCalc;
        #endregion

        public ErrorIndexingManager()
        {
            _dao = new ErrorIndexingDAO();
            _dicErrIndicators = Utilities.GetMappedErrorIndicator();
            _listErrIndicatorsWithoutCalc = Utilities.GetErrorIndicatorWithoutCalculation();
        }

        public void StartErrorIndexCalculation(string prodProgId)
        {
            try
            {
                CommonUtilities.CreateInfoLog("ErrorIndexingManager.StartErrorIndexCalculation() Start StartErrorIndexCalculation() prodProdId: " + prodProgId, 0);
                // TODO: if we use it as a WindowsService
                // CommonUtilities.WriteHealthCheckTimeStamp(WinServiceHealthCheckFileTypesEnum.ImageDeleteService);    
                
                // Reset
                int totalIndicatorWeight = 0;

                foreach (var errIndicator in _dicErrIndicators)
                {
                    #region variables for the calculation
                    DataSet dsErrorIndexCalculation;
                    DataTable dtValuesFromLastCycles;
                    DataTable dtWeightageConfiguration;
                    int indicatorWeight;
                    int deviationFactor;
                    double deviationTop;
                    double deviationBottom;
                    bool indicatorIsActive;
                    string weightConfigurationId;
                    #endregion

                    dsErrorIndexCalculation =  _dao.GetValuesForErrorIndexIndicator(prodProgId, errIndicator.Key, errIndicator.Value);
                    dtValuesFromLastCycles = dsErrorIndexCalculation.Tables[0]; // contains the last 10 or X values from db
                    dtWeightageConfiguration = dsErrorIndexCalculation.Tables[1];
                    if (dsErrorIndexCalculation.Tables[2].Rows.Count < 1 || dsErrorIndexCalculation.Tables[2].Rows[0].IsNull(0) || String.IsNullOrEmpty(dsErrorIndexCalculation.Tables[2].Rows[0][0].ToString()))
                        continue;
                    
                    double lastCycleValue = double.Parse(dsErrorIndexCalculation.Tables[2].Rows[0][0].ToString());
                    
                    ReadIndicatorFieldsToString(dtWeightageConfiguration, out weightConfigurationId, out indicatorWeight, out deviationFactor, out indicatorIsActive);
                    List<double> listValuesFromLastCycles = ReadFieldsToList(dtValuesFromLastCycles);
                    
                    // get the standard deviation
                    double deviation = Utilities.StandardDeviation(listValuesFromLastCycles);

                    Utilities.CalculateDeviationRange(listValuesFromLastCycles, deviationFactor, deviation, out deviationTop, out deviationBottom);

                    // check if the lastValue is inside outside the range
                    bool issueOccurred = Utilities.LastValueIsOutsideRange(lastCycleValue, deviationTop, deviationBottom);

                    if (issueOccurred)
                    {
                        // update PAProductProgramErrorIndex isOccured, average, deviation
                        _dao.UpdatePa_PPErrorIndex(prodProgId, weightConfigurationId, errIndicator.Key, issueOccurred, listValuesFromLastCycles.Average(), deviation, lastCycleValue);

                        if (indicatorIsActive)
                        {
                            // add the weight for this indicator, used for the Priority calculation
                            totalIndicatorWeight += indicatorWeight;
                        }
                    }
                }// end foreach --> // go the next indication (GetWeightageConfigToReportLog)
                
                // add the weight for these indicators which occured but no calculation is required
                foreach (var errIndicator in _listErrIndicatorsWithoutCalc)
                {
                    totalIndicatorWeight += _dao.GetIndicatorWeight(prodProgId, errIndicator);
                }
                

                // calulate the priority for the productProgram table
                DataTable dtCalculationValues = _dao.GetValuesForErrorIndexCalculation(prodProgId);
                double revenueFactorForErrorIndex;
                double qiFactorForErrorIndex;
                double lastMonthPublisherCommission;
                ReadCalculationValuesToString(dtCalculationValues, out revenueFactorForErrorIndex, out qiFactorForErrorIndex, out lastMonthPublisherCommission);

                decimal priority = CalculatePriority(totalIndicatorWeight, qiFactorForErrorIndex, revenueFactorForErrorIndex, lastMonthPublisherCommission);
                _dao.UpdateProductProgram(prodProgId, priority, totalIndicatorWeight);

            }
            catch (AffiliGenericException aex)
            {
                aex.CreateLog();
            }
            catch (Exception ex)
            {
                new AffiliGenericException("Error in ErrorIndexingManager.StartErrorIndexCalculation() PPID " + prodProgId, ex).CreateLog();
            } 
         }


        /// <summary>
        /// Calculates the priority for a program (by a formular from Rene Rath)
        /// </summary>
        /// <returns>The priority</returns>
        private static decimal CalculatePriority(int totalIndicatorWeight, double qiFactorForErrorIndex, double revenueFactorForErrorIndex, double lastMonthPublisherCommission)
        {
            decimal priotity = -1;
            try
            {
                if (totalIndicatorWeight == 0)  // if no error occured totalIndicatorWeight should be 0, no calcaulation required
                    priotity = 0;
                else
                {
                    double result1 = (Math.Log(totalIndicatorWeight) * qiFactorForErrorIndex);
                    if (result1 < 0)
                        result1 = 0;

                    double result2 = (Math.Log(lastMonthPublisherCommission) * revenueFactorForErrorIndex);
                    if (result2 < 0)
                        result2 = 0;

                    // add the results
                    priotity = (decimal)(result1 + result2);
                    // round it
                    priotity = Math.Round(priotity, 3);
    
                }
            }
            catch (Exception ex)
            {
                new AffiliGenericException("ErrorIndexingManager.CalculatePriority(). Error details: ", ex).CreateLog();
            }

            return priotity;    
            
        }

        /// <summary>
        /// Reads the dt fields into global variables
        /// </summary>
        /// <param name="dtErrorIndexCalculation"></param>
        /// <param name="lastMonthPublisherCommission"></param>
        /// <param name="qiFactorForErrorIndex"></param>
        /// <param name="revenueFactorForErrorIndex"></param>
        protected void ReadCalculationValuesToString(DataTable dtErrorIndexCalculation, out double revenueFactorForErrorIndex, out double qiFactorForErrorIndex, out double lastMonthPublisherCommission)
        {
            try
            {
                // this datatable has only 1 row !
                DataRow dr = dtErrorIndexCalculation.Rows[0];

                revenueFactorForErrorIndex = double.Parse(dr[Constants.ErrorIndexCalculation.RevenueFactor_ForErrorIndex].ToString());
                qiFactorForErrorIndex = double.Parse(dr[Constants.ErrorIndexCalculation.QIFactor_ForErrorIndex].ToString());
                lastMonthPublisherCommission = double.Parse(dr[Constants.ErrorIndexCalculation.LastMonthPublisherCommission].ToString());
                
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("ErrorIndexingManager.ReadCalculationValuesToString(). (Wrong settings ind Pa_WeightageConfiguration table.) Error details: ", ex);
            }
        }

        /// <summary>
        /// Reads the dt fields into global variables
        /// </summary>
        /// <param name="dtWeightageConfiguration"></param>
        /// <param name="indicatorWeight"></param>
        /// <param name="deviationFactor"></param>
        /// <param name="indicatorIsActive"></param>
        /// <param name="weightConfigurationId"></param>
        protected void ReadIndicatorFieldsToString(DataTable dtWeightageConfiguration,out string weightConfigurationId, out int indicatorWeight, out int deviationFactor, out bool indicatorIsActive)
        {
            try
            {
                // this datatable has only 1 row !
                DataRow dr = dtWeightageConfiguration.Rows[0];

                weightConfigurationId = dr[Constants.WeightageConfiguration.WeightConfigurationId].ToString();
                indicatorWeight = int.Parse(dr[Constants.WeightageConfiguration.Weight].ToString());
                deviationFactor = int.Parse(dr[Constants.WeightageConfiguration.DeviationFactor].ToString());
                indicatorIsActive = bool.Parse(dr[Constants.Pa_ProductProgramErrorIndex.Activated].ToString());
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("ErrorIndexingManager.ReadIndicatorFieldsToString(). (Wrong settings ind Pa_WeightageConfiguration table.) Error details: ", ex);
            }
        }

        /// <summary>
        /// Converts all entries in a datatable into a list if type double 
        /// </summary>
        /// <param name="dt">Datatable with values to parse into double</param>
        /// <returns>List of type double</returns>
        protected List<double> ReadFieldsToList(DataTable dt)
        {
            try
            {
                List<double> list = new List<double>();
                foreach (DataRow dr in dt.Rows)
                {
                    double parsedValue;
                    if (dr[0] != DBNull.Value && double.TryParse(dr[0].ToString(), out parsedValue))
                    {
                        list.Add(parsedValue);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("ErrorIndexingManager.ReadFieldsToList(). Error details: ", ex);
            }
            
        }
    }
}
