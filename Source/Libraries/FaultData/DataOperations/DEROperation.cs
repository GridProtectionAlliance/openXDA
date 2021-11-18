//******************************************************************************************************
//  DEROperation.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  11/01/2021 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;

namespace FaultData.DataOperations
{
    public class DEROperation : DataOperationBase<MeterDataSet>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DEROperation));

        private double Frequency { get; set; } = 60.0;

        private MeterDataSet MeterDataSet { get; set; }

        public override void Execute(MeterDataSet meterDataSet)
        {
            MeterDataSet = meterDataSet;
            try
            {
                List<DER> ders;
                using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
                {
                    ders = new TableOperations<DER>(connection).QueryRecordsWhere("ID IN (SELECT AssetID FROM MeterAsset WHERE MeterID = {0})", meterDataSet.Meter.ID).ToList();
                    Frequency = double.Parse(connection.ExecuteScalar<string>("SELECT Value FROM SETTING WHERE Name = 'DataAnalysis.SystemFrequency'")?.ToString() ?? "60.0");
                }

                if (ders?.Any() ?? false)
                {
                    foreach(DER der in ders)
                    {
                        CheckRegulation_7_1(der);
                        CheckRegulation_7_2_2(der);
                        CheckRegulation_7_2_3_PLT(der);
                        CheckRegulation_7_2_3_PST(der);
                        CheckRegulation_7_3_h_lessthan_11(der);
                        CheckRegulation_7_3_11_lessthanorequalto_h_lessthan_17(der);
                        CheckRegulation_7_3_17_lessthanorequalto_h_lessthan_23(der);
                        CheckRegulation_7_3_23_lessthanorequalto_h_lessthan_35(der);
                        CheckRegulation_7_3_35_lessthanorequalto_h_lessthan_50(der);
                        CheckRegulation_7_3_h_2(der);
                        CheckRegulation_7_3_h_4(der);
                        CheckRegulation_7_3_h_6(der);
                        CheckRegulation_7_3_TRD(der);
                        CheckRegulation_7_4_1(der);
                        CheckRegulation_7_4_2(der);
                    }
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                Log.Error($"Failed while running DER Operation for {meterDataSet.Meter.AssetKey} - {ex.Message}");
            }
        }

        /* Regulation:  7.1 Limitation of dc injection
           Description: The DER shall not inject dc current greater than 0.5% of the full rated output current at the reference point of applicability(RPA).
           Parameter:   Idc %
           Value:       0.5%
        */
        private void CheckRegulation_7_1(DER der)
        {
            try
            {
                List<Channel> channels;

                using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
                {
                    // Query channels associated with the DC Harmonic component
                    channels = new TableOperations<Channel>(connection).QueryRecordsWhere(@"
                        MeterID = {0} AND 
                        AssetID = {1} AND 
                        MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Current') AND 
                        MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup') AND 
                        HarmonicGroup = 0
                    ", MeterDataSet.Meter.ID, der.ID).ToList();
                }

                if (channels?.Any() ?? false)
                { 
                    foreach(Channel channel in channels)
                    {
                        // Get the data series associated with the channel and choose the highest Series type to perform calculation
                        List<DataSeries> dataSerieses = MeterDataSet.DataSeries.Where(x => x.SeriesInfo.ChannelID == channel.ID).ToList();
                        DataSeries dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Maximum");
                        if(dataSeries == null) dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Average");
                        if (dataSeries == null) dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Minimum");
                        // Probably not properlly configured
                        if (dataSeries == null) continue;

                        // Convert to percent and find datapoints that have exceded threshold
                        List<DataPoint> dataPoints = dataSeries.DataPoints;
                        dataPoints = dataPoints.Select(dp => new DataPoint() { Time = dp.Time, Value = dp.Value * 100 / der.FullRatedOutputCurrent }).ToList();
                        dataPoints = dataPoints.Where(dp => dp.Value > 0.5).ToList();

                        // Load an analysis record for each datapoint that exceded the threshold
                        foreach(DataPoint dataPoint in dataPoints)
                        {
                            DERAnalyticResult result = new DERAnalyticResult() {
                                AssetID = der.ID,
                                ChannelID = channel.ID,
                                DataType = "Trend",
                                MeterID = channel.MeterID,
                                Parameter = "Idc %",
                                Regulation = "7.1 Limitation of dc injection",
                                Threshold = 0.5,
                                Time = dataPoint.Time,
                                Value = dataPoint.Value
                            };

                            LoadResult(result);
                        }
                    }
                }
                else
                    return;

            }
            catch (Exception ex)
            {
                throw new Exception($"7.1 - {ex.Message}");
            }

        }

        /* Regulation:  7.2.2 Rapid Voltage Change (RVC)
           Description: When the PCC is at medium voltage, the DER shall not cause step or ramp changes in the RMS voltage at the PCC exceeding 3% of nominal and exceeding 3% per 
                        second averaged over a period of one second.  When the PCC is at low voltage, the DER shall not cause step or ramp changes in the RMS voltage exceeding 5% 
                        of nominal and exceeding 5% per second averaged over a period of one second. Any exception to the limits is subject to approval by the Area EPS operator 
                        with consideration of other sources of RVC within the Area EPS. These RVC limits shall apply to sudden changes due to frequent energization of transformers, 
                        frequent switching of capacitors or from abrupt output variations caused by DER misoperation.These RVC limits shall not apply to infrequent events such as 
                        switching, unplanned tripping, or transformer energization related to commissioning, fault restoration, or maintenance.
           Parameter:   Vrms %
           Value:       3% (Medium Voltage), 5% (Low Voltage)
        */
        private void CheckRegulation_7_2_2(DER der)
        {
            try {
                List<Channel> channels;
                int? eventID;
                using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
                {
                    // Query channels associated with the DC Harmonic component
                    channels = new TableOperations<Channel>(connection).QueryRecordsWhere(@"
                        MeterID = {0} AND 
                        AssetID = {1} AND 
                        MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND 
                        MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Instantaneous')
                    ", MeterDataSet.Meter.ID, der.ID).ToList();

                    eventID = connection.ExecuteScalar<int?>("SELECT TOP 1 ID FROM Event WHERE MeterID = {0} AND AssetID = {1} AND FileGroupID = {2}", MeterDataSet.Meter.ID, der.ID, MeterDataSet.FileGroup.ID);

                }

                if (channels?.Any() ?? false)
                {
                    foreach (Channel channel in channels)
                    {
                        // Get the data series associated with the channel and choose the highest Series type to perform calculation
                        List<DataSeries> dataSerieses = MeterDataSet.DataSeries.Where(x => x.SeriesInfo.ChannelID == channel.ID).ToList();
                        DataSeries dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Values");
                        // Probably not properlly configured
                        if (dataSeries == null) continue;

                        // Convert to RVC and find datapoints that have exceded threshold
                        List<DataPoint> dataPoints = Transform.ToRMS(dataSeries, Frequency).DataPoints;
                        dataPoints = dataPoints.Select((dp,index) => {
                            double current = dp.Value;
                            double previous = dp.Value;
                            if (index > 0) previous = dataPoints[index - 1].Value;
                            double rvc = Math.Abs(current / previous - 1) * 100;
                            return new DataPoint() { Time = dp.Time, Value = rvc };
                            
                         }).ToList();

                        double threshold = 5;
                        if (der.VoltageLevel == "Medium") threshold = 3;

                         dataPoints = dataPoints.Where(dp => dp.Value > threshold).ToList();
                        // Load an analysis record for each datapoint that exceded the threshold
                        foreach (DataPoint dataPoint in dataPoints)
                        {
                            DERAnalyticResult result = new DERAnalyticResult()
                            {
                                AssetID = der.ID,
                                ChannelID = channel.ID,
                                EventID = eventID,
                                DataType = "Event",
                                MeterID = channel.MeterID,
                                Parameter = "Vrms %",
                                Regulation = "7.2.2 Rapid Voltage Change (RVC)",
                                Threshold = threshold,
                                Time = dataPoint.Time,
                                Value = dataPoint.Value
                            };

                            LoadResult(result);
                        }
                    }
                }
                else
                    return;

            }
            catch (Exception ex)
            {
                throw new Exception($"7.2.2 - {ex.Message}");
            }
        }

        /* Regulation: 7.2.3 Flicker (PST)
           Description: The DER contribution (emission values) to the flicker, measured at the PCC, shall not exceed the greater of the limits listed in Table 25 and the individual emission
                        limits defined by IEC/TR 61000-3-7. Any exception to the limits shall be approved by the Area EPS operator with consideration of other sources of flicker within the 
                        Area EPS.  
                        Note: The 95% probability value should not exceed the emission limit based on a one week measurement period

           Parameter: PST
           Value 0.35
        */
        private void CheckRegulation_7_2_3_PST(DER der)
        {
            try
            {
                List<Channel> channels;

                using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
                {
                    // Query channels associated with the FlkrPST
                    channels = new TableOperations<Channel>(connection).QueryRecordsWhere(@"
                        MeterID = {0} AND 
                        AssetID = {1} AND 
                        MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Current') AND 
                        MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPST') 
                    ", MeterDataSet.Meter.ID, der.ID).ToList();
                }

                if (channels?.Any() ?? false)
                {
                    foreach (Channel channel in channels)
                    {
                        // Get the data series associated with the channel and choose the highest Series type to perform calculation
                        List<DataSeries> dataSerieses = MeterDataSet.DataSeries.Where(x => x.SeriesInfo.ChannelID == channel.ID).ToList();
                        DataSeries dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Maximum");
                        if (dataSeries == null) dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Average");
                        if (dataSeries == null) dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Minimum");
                        // Probably not properlly configured
                        if (dataSeries == null) continue;

                        //  find datapoints that have exceded threshold
                        List<DataPoint> dataPoints = dataSeries.DataPoints;
                        dataPoints = dataPoints.Where(dp => dp.Value > 0.35).ToList();

                        // Load an analysis record for each datapoint that exceded the threshold
                        foreach (DataPoint dataPoint in dataPoints)
                        {
                            DERAnalyticResult result = new DERAnalyticResult()
                            {
                                AssetID = der.ID,
                                ChannelID = channel.ID,
                                DataType = "Trend",
                                MeterID = channel.MeterID,
                                Parameter = "PST",
                                Regulation = "7.2.3 Flicker",
                                Threshold = 0.35,
                                Time = dataPoint.Time,
                                Value = dataPoint.Value
                            };

                            LoadResult(result);
                        }
                    }
                }
                else
                    return;


            }
            catch (Exception ex)
            {
                throw new Exception($"7.2.3 PST - {ex.Message}");
            }

        }

        /* Regulation: 7.2.3 Flicker (PLT)
           Description: The DER contribution (emission values) to the flicker, measured at the PCC, shall not exceed the greater of the limits listed in Table 25 and the individual emission
                        limits defined by IEC/TR 61000-3-7. Any exception to the limits shall be approved by the Area EPS operator with consideration of other sources of flicker within the 
                        Area EPS.  
                        Note: The 95% probability value should not exceed the emission limit based on a one week measurement period

           Parameter: PLT
           Value 0.25
        */
        private void CheckRegulation_7_2_3_PLT(DER der)
        {
            try
            {
                List<Channel> channels;

                using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
                {
                    // Query channels associated with the FlkrPLT
                    channels = new TableOperations<Channel>(connection).QueryRecordsWhere(@"
                        MeterID = {0} AND 
                        AssetID = {1} AND 
                        MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Current') AND 
                        MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPLT') 
                    ", MeterDataSet.Meter.ID, der.ID).ToList();
                }

                if (channels?.Any() ?? false)
                {
                    foreach (Channel channel in channels)
                    {
                        // Get the data series associated with the channel and choose the highest Series type to perform calculation
                        List<DataSeries> dataSerieses = MeterDataSet.DataSeries.Where(x => x.SeriesInfo.ChannelID == channel.ID).ToList();
                        DataSeries dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Maximum");
                        if (dataSeries == null) dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Average");
                        if (dataSeries == null) dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Minimum");
                        // Probably not properlly configured
                        if (dataSeries == null) continue;

                        //  find datapoints that have exceded threshold
                        List<DataPoint> dataPoints = dataSeries.DataPoints;
                        dataPoints = dataPoints.Where(dp => dp.Value > 0.25).ToList();

                        // Load an analysis record for each datapoint that exceded the threshold
                        foreach (DataPoint dataPoint in dataPoints)
                        {
                            DERAnalyticResult result = new DERAnalyticResult()
                            {
                                AssetID = der.ID,
                                ChannelID = channel.ID,
                                DataType = "Trend",
                                MeterID = channel.MeterID,
                                Parameter = "PLT",
                                Regulation = "7.2.3 Flicker",
                                Threshold = 0.25,
                                Time = dataPoint.Time,
                                Value = dataPoint.Value
                            };

                            LoadResult(result);
                        }
                    }
                }
                else
                    return;


            }
            catch (Exception ex)
            {
                throw new Exception($"7.2.3 PLT - {ex.Message}");
            }

        }

        /* Regulation: 7.3 Limitation of current distoriation
           Description: Harmonic current distortion, inter-harmonic current distortion, and total rated-current distortion (TRD) at the reference point of applicability (RPA) shall not exceed 
                        the limits stated in the following paragraph and in Table 26 and Table 27.  The methodology for measuring harmonic and inter-harmonic values in this requirement is defined 
                        in IEEE Std 519.108 Note that Table 26 and Table 27 differ from any table in IEEE Std 519. In this standard, the new term “Total Rated-current Distortion (TRD)” was 
                        introduced and used instead of TDD (in Table 26) and the even order current distortion limits above the second order are relaxed for DER (in Table 27). Any aggregated 
                        harmonics current distortion between h ± 5 Hz, where h is the individual harmonic order, shall be limited to the associated harmonic order h limit in Table 26 and Table 27. 
                        Any aggregated interharmonics current distortion between h + 5 Hz and (h + 1) − 5 Hz shall be limited to the lesser magnitude limit of h and h + 1 harmonic order in 
                        Table 26 and Table 27."
           Parameter: h < 11
           Value 4
        */
        private void CheckRegulation_7_3_h_lessthan_11(DER der)
        {
            try
            {
                CheckRegulation_7_3(der, 1, 11, 4);
            }
            catch (Exception ex)
            {
                throw new Exception($"7.3 h < 11 - {ex.Message}");
            }

        }

        /* Regulation: 7.3 Limitation of current distoriation
           Description: Harmonic current distortion, inter-harmonic current distortion, and total rated-current distortion (TRD) at the reference point of applicability (RPA) shall not exceed 
                        the limits stated in the following paragraph and in Table 26 and Table 27.  The methodology for measuring harmonic and inter-harmonic values in this requirement is defined 
                        in IEEE Std 519.108 Note that Table 26 and Table 27 differ from any table in IEEE Std 519. In this standard, the new term “Total Rated-current Distortion (TRD)” was 
                        introduced and used instead of TDD (in Table 26) and the even order current distortion limits above the second order are relaxed for DER (in Table 27). Any aggregated 
                        harmonics current distortion between h ± 5 Hz, where h is the individual harmonic order, shall be limited to the associated harmonic order h limit in Table 26 and Table 27. 
                        Any aggregated interharmonics current distortion between h + 5 Hz and (h + 1) − 5 Hz shall be limited to the lesser magnitude limit of h and h + 1 harmonic order in 
                        Table 26 and Table 27."
           Parameter: 11 <= h < 17
           Value 2
        */
        private void CheckRegulation_7_3_11_lessthanorequalto_h_lessthan_17(DER der)
        {
            try
            {
                CheckRegulation_7_3(der, 11, 17, 2);
            }
            catch (Exception ex)
            {
                throw new Exception($"7.3 11 <= h < 17 - {ex.Message}");
            }


        }

        /* Regulation: 7.3 Limitation of current distoriation
           Description: Harmonic current distortion, inter-harmonic current distortion, and total rated-current distortion (TRD) at the reference point of applicability (RPA) shall not exceed 
                        the limits stated in the following paragraph and in Table 26 and Table 27.  The methodology for measuring harmonic and inter-harmonic values in this requirement is defined 
                        in IEEE Std 519.108 Note that Table 26 and Table 27 differ from any table in IEEE Std 519. In this standard, the new term “Total Rated-current Distortion (TRD)” was 
                        introduced and used instead of TDD (in Table 26) and the even order current distortion limits above the second order are relaxed for DER (in Table 27). Any aggregated 
                        harmonics current distortion between h ± 5 Hz, where h is the individual harmonic order, shall be limited to the associated harmonic order h limit in Table 26 and Table 27. 
                        Any aggregated interharmonics current distortion between h + 5 Hz and (h + 1) − 5 Hz shall be limited to the lesser magnitude limit of h and h + 1 harmonic order in 
                        Table 26 and Table 27."
           Parameter: 17 <= h < 23
           Value 1.5
        */
        private void CheckRegulation_7_3_17_lessthanorequalto_h_lessthan_23(DER der)
        {
            try
            {
                CheckRegulation_7_3(der, 17, 23, 1.5);
            }
            catch (Exception ex)
            {
                throw new Exception($"7.3 17 <= h < 23 - {ex.Message}");
            }

        }

        /* Regulation: 7.3 Limitation of current distoriation
           Description: Harmonic current distortion, inter-harmonic current distortion, and total rated-current distortion (TRD) at the reference point of applicability (RPA) shall not exceed 
                        the limits stated in the following paragraph and in Table 26 and Table 27.  The methodology for measuring harmonic and inter-harmonic values in this requirement is defined 
                        in IEEE Std 519.108 Note that Table 26 and Table 27 differ from any table in IEEE Std 519. In this standard, the new term “Total Rated-current Distortion (TRD)” was 
                        introduced and used instead of TDD (in Table 26) and the even order current distortion limits above the second order are relaxed for DER (in Table 27). Any aggregated 
                        harmonics current distortion between h ± 5 Hz, where h is the individual harmonic order, shall be limited to the associated harmonic order h limit in Table 26 and Table 27. 
                        Any aggregated interharmonics current distortion between h + 5 Hz and (h + 1) − 5 Hz shall be limited to the lesser magnitude limit of h and h + 1 harmonic order in 
                        Table 26 and Table 27."
           Parameter: 23 <= h < 35
           Value 0.6
        */
        private void CheckRegulation_7_3_23_lessthanorequalto_h_lessthan_35(DER der)
        {
            try
            {
                CheckRegulation_7_3(der, 23, 35, 0.6);
            }
            catch (Exception ex)
            {
                throw new Exception($"7.3 23 <= h < 35 - {ex.Message}");
            }

        }

        /* Regulation: 7.3 Limitation of current distoriation
           Description: Harmonic current distortion, inter-harmonic current distortion, and total rated-current distortion (TRD) at the reference point of applicability (RPA) shall not exceed 
                        the limits stated in the following paragraph and in Table 26 and Table 27.  The methodology for measuring harmonic and inter-harmonic values in this requirement is defined 
                        in IEEE Std 519.108 Note that Table 26 and Table 27 differ from any table in IEEE Std 519. In this standard, the new term “Total Rated-current Distortion (TRD)” was 
                        introduced and used instead of TDD (in Table 26) and the even order current distortion limits above the second order are relaxed for DER (in Table 27). Any aggregated 
                        harmonics current distortion between h ± 5 Hz, where h is the individual harmonic order, shall be limited to the associated harmonic order h limit in Table 26 and Table 27. 
                        Any aggregated interharmonics current distortion between h + 5 Hz and (h + 1) − 5 Hz shall be limited to the lesser magnitude limit of h and h + 1 harmonic order in 
                        Table 26 and Table 27."
           Parameter: 35 <= h < 50
           Value 0.3
        */
        private void CheckRegulation_7_3_35_lessthanorequalto_h_lessthan_50(DER der)
        {
            try
            {
                CheckRegulation_7_3(der, 35, 50, 0.3);
            }
            catch (Exception ex)
            {
                throw new Exception($"7.3 35 <= h < 50 - {ex.Message}");
            }

        }


        /* Regulation: 7.3 Limitation of current distoriation
           Description: Harmonic current distortion, inter-harmonic current distortion, and total rated-current distortion (TRD) at the reference point of applicability (RPA) shall not exceed 
                        the limits stated in the following paragraph and in Table 26 and Table 27.  The methodology for measuring harmonic and inter-harmonic values in this requirement is defined 
                        in IEEE Std 519.108 Note that Table 26 and Table 27 differ from any table in IEEE Std 519. In this standard, the new term “Total Rated-current Distortion (TRD)” was 
                        introduced and used instead of TDD (in Table 26) and the even order current distortion limits above the second order are relaxed for DER (in Table 27). Any aggregated 
                        harmonics current distortion between h ± 5 Hz, where h is the individual harmonic order, shall be limited to the associated harmonic order h limit in Table 26 and Table 27. 
                        Any aggregated interharmonics current distortion between h + 5 Hz and (h + 1) − 5 Hz shall be limited to the lesser magnitude limit of h and h + 1 harmonic order in 
                        Table 26 and Table 27."
           Parameter: TRD
           Value 5
        */
        private void CheckRegulation_7_3_TRD(DER der)
        {
            try
            {
                CheckRegulation_7_3(der, 1, 50, 5);
            }
            catch (Exception ex)
            {
                throw new Exception($"7.3 TRD - {ex.Message}");
            }

        }

        /* Regulation: 7.3 Limitation of current distoriation
           Description: Harmonic current distortion, inter-harmonic current distortion, and total rated-current distortion (TRD) at the reference point of applicability (RPA) shall not exceed 
                        the limits stated in the following paragraph and in Table 26 and Table 27.  The methodology for measuring harmonic and inter-harmonic values in this requirement is defined 
                        in IEEE Std 519.108 Note that Table 26 and Table 27 differ from any table in IEEE Std 519. In this standard, the new term “Total Rated-current Distortion (TRD)” was 
                        introduced and used instead of TDD (in Table 26) and the even order current distortion limits above the second order are relaxed for DER (in Table 27). Any aggregated 
                        harmonics current distortion between h ± 5 Hz, where h is the individual harmonic order, shall be limited to the associated harmonic order h limit in Table 26 and Table 27. 
                        Any aggregated interharmonics current distortion between h + 5 Hz and (h + 1) − 5 Hz shall be limited to the lesser magnitude limit of h and h + 1 harmonic order in 
                        Table 26 and Table 27."
           Parameter: Even, h=2
           Value 1
        */
        private void CheckRegulation_7_3_h_2(DER der)
        {
            try
            {
                CheckRegulation_7_3_Even(der, 2, 1);
            }
            catch (Exception ex)
            {
                throw new Exception($"7.3 h = 2 - {ex.Message}");
            }

        }

        /* Regulation: 7.3 Limitation of current distoriation
           Description: Harmonic current distortion, inter-harmonic current distortion, and total rated-current distortion (TRD) at the reference point of applicability (RPA) shall not exceed 
                        the limits stated in the following paragraph and in Table 26 and Table 27.  The methodology for measuring harmonic and inter-harmonic values in this requirement is defined 
                        in IEEE Std 519.108 Note that Table 26 and Table 27 differ from any table in IEEE Std 519. In this standard, the new term “Total Rated-current Distortion (TRD)” was 
                        introduced and used instead of TDD (in Table 26) and the even order current distortion limits above the second order are relaxed for DER (in Table 27). Any aggregated 
                        harmonics current distortion between h ± 5 Hz, where h is the individual harmonic order, shall be limited to the associated harmonic order h limit in Table 26 and Table 27. 
                        Any aggregated interharmonics current distortion between h + 5 Hz and (h + 1) − 5 Hz shall be limited to the lesser magnitude limit of h and h + 1 harmonic order in 
                        Table 26 and Table 27."
           Parameter: Even, h=4
           Value 2
        */
        private void CheckRegulation_7_3_h_4(DER der)
        {
            try
            {
                CheckRegulation_7_3_Even(der, 4, 2);
            }
            catch (Exception ex)
            {
                throw new Exception($"7.3 h = 4 - {ex.Message}");
            }
        }

        /* Regulation: 7.3 Limitation of current distoriation
           Description: Harmonic current distortion, inter-harmonic current distortion, and total rated-current distortion (TRD) at the reference point of applicability (RPA) shall not exceed 
                        the limits stated in the following paragraph and in Table 26 and Table 27.  The methodology for measuring harmonic and inter-harmonic values in this requirement is defined 
                        in IEEE Std 519.108 Note that Table 26 and Table 27 differ from any table in IEEE Std 519. In this standard, the new term “Total Rated-current Distortion (TRD)” was 
                        introduced and used instead of TDD (in Table 26) and the even order current distortion limits above the second order are relaxed for DER (in Table 27). Any aggregated 
                        harmonics current distortion between h ± 5 Hz, where h is the individual harmonic order, shall be limited to the associated harmonic order h limit in Table 26 and Table 27. 
                        Any aggregated interharmonics current distortion between h + 5 Hz and (h + 1) − 5 Hz shall be limited to the lesser magnitude limit of h and h + 1 harmonic order in 
                        Table 26 and Table 27."
           Parameter: Even, h=6
           Value 3
        */
        private void CheckRegulation_7_3_h_6(DER der)
        {
            try
            {
                CheckRegulation_7_3_Even(der, 6, 3);
            }
            catch (Exception ex)
            {
                throw new Exception($"7.3 h = 6- {ex.Message}");
            }

        }

        private void CheckRegulation_7_3(DER der, int lowHarmonic, int highHarmonic, double threshold) {

            List<Channel> channels;

            using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
            {
                // Query channels associated with the FlkrPLT
                channels = new TableOperations<Channel>(connection).QueryRecordsWhere(@"
                        MeterID = {0} AND 
                        AssetID = {1} AND 
                        MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Current') AND 
                        MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup') AND
                        HarmonicGroup > {2} AND
                        HarmonicGroup < {3}
                    ", MeterDataSet.Meter.ID, der.ID, lowHarmonic, highHarmonic).ToList();
            }

            if (channels?.Any() ?? false)
            {
                IEnumerable<IGrouping<int, Channel>> phasedChannels = channels.GroupBy(c => c.PhaseID);
                foreach (IGrouping<int, Channel> phase in phasedChannels)
                {
                    Channel rms;
                    using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
                    {
                        // Query channels associated with the FlkrPLT
                        rms = new TableOperations<Channel>(connection).QueryRecordWhere(@"
                                MeterID = {0} AND 
                                AssetID = {1} AND 
                                MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Current') AND 
                                MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND
                                PhaseID = {2}
                            ", MeterDataSet.Meter.ID, der.ID, phase.Key);
                    }

                    // if rms doesn't exist we cant caculate TRD, continue;
                    if (rms == null) continue;

                    List<DataPoint> rmsDataPoints = MeterDataSet.DataSeries.Find(s => s.SeriesInfo.ChannelID == rms.ID && s.SeriesInfo.SeriesType.Name == "Average")?.DataPoints;

                    // if rms datapoints do not exist we cant caculate TRD, continue;
                    if (rmsDataPoints == null) continue;

                    // Get the data series associated with the channel and choose the highest Series type to perform calculation
                    List<DataSeries> dataSerieses = MeterDataSet.DataSeries.Where(x => phase.Where(p => p.ID == x.SeriesInfo.ChannelID).Any()).ToList();
                    if (dataSerieses.Count == 0) continue;

                    IEnumerable<IGrouping<int, DataSeries>> channeledSeries = dataSerieses.GroupBy(c => c.SeriesInfo.ChannelID);
                    List<DataSeries> selectedSeries = new List<DataSeries>();
                    foreach (IGrouping<int, DataSeries> series in channeledSeries)
                    {
                        DataSeries dataSeries = series.ToList().Find(x => x.SeriesInfo.SeriesType.Name == "Maximum");
                        if (dataSeries == null) dataSeries = series.ToList().Find(x => x.SeriesInfo.SeriesType.Name == "Average");
                        if (dataSeries == null) dataSeries = series.ToList().Find(x => x.SeriesInfo.SeriesType.Name == "Minimum");
                        // Probably not properlly configured
                        if (dataSeries == null) continue;

                        selectedSeries.Add(dataSeries);
                    }

                    //  sum up the selected series, calculate  and find datapoints that have exceded threshold
                    IEnumerable<DataPoint> dataPoints = selectedSeries.SelectMany(s => s.DataPoints).GroupBy(dp => dp.Time).Select(dp => new DataPoint() { Time = dp.Key, Value = dp.Sum(d => d.Value) });
                    dataPoints = dataPoints.Join(rmsDataPoints, harmonicDP => harmonicDP.Time, rmsDP => rmsDP.Time, (dp, rmsDp) => new DataPoint() { Time = dp.Time, Value = Math.Sqrt(Math.Abs(Math.Pow(dp.Value, 2) - Math.Pow(rmsDp.Value, 2))) /der.FullRatedOutputCurrent * 100 });
                    dataPoints = dataPoints.Where(dp => dp.Value > threshold).ToList();

                    // Load an analysis record for each datapoint that exceded the threshold
                    foreach (DataPoint dataPoint in dataPoints)
                    {
                        DERAnalyticResult result = new DERAnalyticResult()
                        {
                            AssetID = der.ID,
                            ChannelID = rms.ID,
                            DataType = "Trend",
                            MeterID = MeterDataSet.Meter.ID,
                            Parameter = $"{lowHarmonic} <= h < {highHarmonic}",
                            Regulation = "7.3 Limitation of current distoriation",
                            Threshold = threshold,
                            Time = dataPoint.Time,
                            Value = dataPoint.Value
                        };

                        LoadResult(result);
                    }
                }
            }
            else
                return;


        }

        private void CheckRegulation_7_3_Even(DER der, int harmonic, double threshold)
        {

            List<Channel> channels;

            using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
            {
                // Query channels associated with the FlkrPLT
                channels = new TableOperations<Channel>(connection).QueryRecordsWhere(@"
                        MeterID = {0} AND 
                        AssetID = {1} AND 
                        MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Current') AND 
                        MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup') AND
                        HarmonicGroup >= {2} AND
                        HarmonicGroup % 2 = 0
                    ", MeterDataSet.Meter.ID, der.ID, harmonic).ToList();
            }

            if (channels?.Any() ?? false)
            {
                IEnumerable<IGrouping<int, Channel>> phasedChannels = channels.GroupBy(c => c.PhaseID);
                foreach (IGrouping<int, Channel> phase in phasedChannels)
                {
                    Channel rms;
                    using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
                    {
                        // Query channels associated with the FlkrPLT
                        rms = new TableOperations<Channel>(connection).QueryRecordWhere(@"
                                MeterID = {0} AND 
                                AssetID = {1} AND 
                                MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Current') AND 
                                MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND
                                PhaseID = {2}
                            ", MeterDataSet.Meter.ID, der.ID, phase.Key);
                    }

                    // if rms doesn't exist we cant caculate TRD, continue;
                    if (rms == null) continue;

                    List<DataPoint> rmsDataPoints = MeterDataSet.DataSeries.Find(s => s.SeriesInfo.ChannelID == rms.ID && s.SeriesInfo.SeriesType.Name == "Average")?.DataPoints;

                    // if rms datapoints do not exist we cant caculate TRD, continue;
                    if (rmsDataPoints == null) continue;

                    // Get the data series associated with the channel and choose the highest Series type to perform calculation
                    List<DataSeries> dataSerieses = MeterDataSet.DataSeries.Where(x => phase.Where(p => p.ID == x.SeriesInfo.ChannelID).Any()).ToList();
                    IEnumerable<IGrouping<int, DataSeries>> channeledSeries = dataSerieses.GroupBy(c => c.SeriesInfo.ChannelID);
                    List<DataSeries> selectedSeries = new List<DataSeries>();
                    foreach (IGrouping<int, DataSeries> series in channeledSeries)
                    {
                        DataSeries dataSeries = series.ToList().Find(x => x.SeriesInfo.SeriesType.Name == "Maximum");
                        if (dataSeries == null) dataSeries = series.ToList().Find(x => x.SeriesInfo.SeriesType.Name == "Average");
                        if (dataSeries == null) dataSeries = series.ToList().Find(x => x.SeriesInfo.SeriesType.Name == "Minimum");
                        // Probably not properlly configured
                        if (dataSeries == null) continue;

                        selectedSeries.Add(dataSeries);
                    }

                    //  sum up the selected series, calculate  and find datapoints that have exceded threshold
                    var grouped = selectedSeries.SelectMany(s => s.DataPoints).GroupBy(dp => dp.Time);
                    IEnumerable<DataPoint> dataPoints = grouped.Select(dp => new DataPoint() { Time = dp.Key, Value = dp.Sum(d => d.Value) });
                    dataPoints = dataPoints.Join(rmsDataPoints, harmonicDP => harmonicDP.Time, rmsDP => rmsDP.Time, (dp, rmsDp) => new DataPoint() { Time = dp.Time, Value = Math.Sqrt(Math.Abs(Math.Pow(dp.Value, 2) - Math.Pow(rmsDp.Value, 2))) / der.FullRatedOutputCurrent * 100 });
                    dataPoints = dataPoints.Where(dp => dp.Value > threshold).ToList();

                    // Load an analysis record for each datapoint that exceded the threshold
                    foreach (DataPoint dataPoint in dataPoints)
                    {
                        DERAnalyticResult result = new DERAnalyticResult()
                        {
                            AssetID = der.ID,
                            ChannelID = rms.ID,
                            DataType = "Trend",
                            MeterID = MeterDataSet.Meter.ID,
                            Parameter = $"Even, h >= {harmonic}",
                            Regulation = "7.3 Limitation of current distoriation",
                            Threshold = threshold,
                            Time = dataPoint.Time,
                            Value = dataPoint.Value
                        };

                        LoadResult(result);
                    }
                }
            }
            else
                return;


        }

        /* Regulation: 7.4.1 Limitation of overvoltage over one fundametnal frequency
           Description: The DER shall not cause the fundamental frequency line-to-ground (or line-to-line) voltage on any portion of the Area EPS that is designed to operate effectively grounded, 
                        as defined by IEEE Std C62.92.1, to exceed 138% of its nominal line-to-ground fundamental frequency voltage for a duration exceeding one fundamental frequency period.
           Parameter: 1 Cycle RMS Voltage (pu)
           Value 1.38
        */
        private void CheckRegulation_7_4_1(DER der)
        {
            try
            {
                List<Channel> channels;

                using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
                {
                    // Query channels associated with the DC Harmonic component
                    channels = new TableOperations<Channel>(connection).QueryRecordsWhere(@"
                        MeterID = {0} AND 
                        AssetID = {1} AND 
                        MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND 
                        MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS') AND
                        PhaseID IN (SELECT ID FROM Phase WHERE Name IN ('AN', 'BN', 'CN', 'AB', 'BC', 'CA'))
                    ", MeterDataSet.Meter.ID, der.ID).ToList();
                }

                if (channels?.Any() ?? false)
                {
                    foreach (Channel channel in channels)
                    {
                        // Get the data series associated with the channel and choose the highest Series type to perform calculation
                        List<DataSeries> dataSerieses = MeterDataSet.DataSeries.Where(x => x.SeriesInfo.ChannelID == channel.ID).ToList();
                        DataSeries dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Maximum");
                        if (dataSeries == null) dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Average");
                        if (dataSeries == null) dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Minimum");
                        // Probably not properlly configured
                        if (dataSeries == null) continue;

                        channel.ConnectionFactory = MeterDataSet.CreateDbConnection;

                        // Convert to percent and find datapoints that have exceded threshold
                        List<DataPoint> dataPoints = dataSeries.DataPoints;
                        double nominalVoltage = der.VoltageKV * 1000;

                        if (new List<string>() { "AN", "BN", "CN" }.IndexOf(channel.Phase.Name) >= 0)
                            nominalVoltage = nominalVoltage / Math.Sqrt(3);

                        dataPoints = dataPoints.Select(dp => new DataPoint() { Time = dp.Time, Value = dp.Value / nominalVoltage }).ToList();
                        dataPoints = dataPoints.Where(dp => dp.Value > 1.38).ToList();

                        // Load an analysis record for each datapoint that exceded the threshold
                        foreach (DataPoint dataPoint in dataPoints)
                        {
                            DERAnalyticResult result = new DERAnalyticResult()
                            {
                                AssetID = der.ID,
                                ChannelID = channel.ID,
                                DataType = "Trend",
                                MeterID = channel.MeterID,
                                Parameter = "1 Cycle RMS Voltage (pu)",
                                Regulation = "7.4.1 Limitation of overvoltage over one fundametnal frequency",
                                Threshold = 1.38,
                                Time = dataPoint.Time,
                                Value = dataPoint.Value
                            };

                            LoadResult(result);
                        }
                    }
                }
                else
                    return;

            }
            catch (Exception ex)
            {
                throw new Exception($"7.4.1 - {ex.Message}");
            }

        }

        /* Regulation: 7.4.2 Limitation of cumulative instantaneous overvoltage
           Description: The DER shall not cause the instantaneous voltage on any portion of the Area EPS to exceed the magnitudes and cumulative durations shown in Figure 3. The cumulative duration 
                        shall only include the sum of durations for which the instantaneous voltage exceeds the respective threshold over a one-minute time window
           Parameter: Instantaneous VPk(pu)
           Value: duration >= 16 ms             1.3
                  3ms <= duration < 16 ms       1.4
                  1.6ms <= duration < 3 ms      1.7
                  0 ms <= duration < 1.6 ms     2
        */
        private void CheckRegulation_7_4_2(DER der)
        {
            try
            {
                List<Channel> channels;
                //double freq;
                int? eventID;
                using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
                {
                    // Query channels
                    channels = new TableOperations<Channel>(connection).QueryRecordsWhere(@"
                        MeterID = {0} AND 
                        AssetID = {1} AND 
                        MeasurementTypeID = (SELECT ID FROM MeasurementType WHERE Name = 'Voltage') AND 
                        MeasurementCharacteristicID = (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Instantaneous') AND
                        PhaseID IN (SELECT ID FROM Phase WHERE Name IN ('AN', 'BN', 'CN', 'AB', 'BC', 'CA'))
                    ", MeterDataSet.Meter.ID, der.ID).ToList();

                    eventID = connection.ExecuteScalar<int?>("SELECT TOP 1 ID FROM Event WHERE MeterID = {0} AND AssetID = {1} AND FileGroupID = {2}", MeterDataSet.Meter.ID, der.ID, MeterDataSet.FileGroup.ID);

                    //freq = double.Parse(connection.ExecuteScalar<string>("SELECT TOP 1 Value Setting WHERE Name = 'DataAnalysis.SystemFrequency'")?.ToString() ?? "60.0");

                }

                if (channels?.Any() ?? false)
                {
                    foreach (Channel channel in channels)
                    {
                        // Get the data series associated with the channel and choose the highest Series type to perform calculation
                        List<DataSeries> dataSerieses = MeterDataSet.DataSeries.Where(x => x.SeriesInfo.ChannelID == channel.ID).ToList();
                        DataSeries dataSeries = dataSerieses.Find(x => x.SeriesInfo.SeriesType.Name == "Values");

                        // Probably not properlly configured
                        if (dataSeries == null) continue;
                        DataSeries rmsData = Transform.ToRMS(dataSeries, Frequency);

                        channel.ConnectionFactory = MeterDataSet.CreateDbConnection;

                        // Convert to percent and find datapoints that have exceded threshold
                        List<DataPoint> dataPoints = rmsData.DataPoints;
                        if (dataPoints.Count == 0) continue;

                        double nominalVoltage = der.VoltageKV * 1000;

                        if (new List<string>() { "AN", "BN", "CN" }.IndexOf(channel.Phase.Name) >= 0)
                            nominalVoltage = nominalVoltage / Math.Sqrt(3);

                        double duration = (dataPoints.First().Time - dataPoints.Last().Time).TotalMilliseconds;
                        //double cycles = duration * freq / 1000;
                        //int sampleRatePerCycle = (int)(dataPoints.Count/cycles);
                        double sampleLengthInMS = duration/dataPoints.Count;

                        dataPoints = dataPoints.Select(dp => new DataPoint() { Time = dp.Time, Value = dp.Value / nominalVoltage }).ToList();
                        List<Tuple<double, double,double>> thresholds = new List<Tuple<double, double, double>>() { 
                            new Tuple<double, double, double>(1.3, 16, 1000000000),
                            new Tuple<double, double, double>(1.4, 3, 16),
                            new Tuple<double, double, double>(1.7, 1.6, 3),
                            new Tuple<double, double, double>(2, 0, 1.6),
                        };

                        foreach(Tuple<double, double, double> threshold in thresholds)
                        {
                            List<DataPoint> points = dataPoints.Where(dp => dp.Value > threshold.Item1).ToList();
                            if (points.Count == 0) continue;

                            double milliseconds = points.Count * sampleLengthInMS;

                            if (threshold.Item2 <= milliseconds && milliseconds < threshold.Item3)
                            {
                                DataPoint max = points.OrderByDescending(x => x.Value).First();

                                LoadResult(eventID, der.ID, MeterDataSet.Meter.ID, channel.ID, max.Time, $"{threshold.Item2} ms (total: {milliseconds})", threshold.Item1, max.Value);
                                // only load max value for the larget number of theshold violations
                                break;
                            }

                        }

                    }
                }
                else
                    return;

            }
            catch (Exception ex)
            {
                throw new Exception($"7.4.2 - {ex.Message}");
            }

        }

        private void LoadResult(DERAnalyticResult result)
        {
            using (AdoDataConnection connection = MeterDataSet.CreateDbConnection()) {
                new TableOperations<DERAnalyticResult>(connection).AddNewRecord(result);
            }
        }

        private void LoadResult(int? eventID, int assetID, int meterID, int channelID, DateTime time, string parameter, double threshold, double value)
        {
            using (AdoDataConnection connection = MeterDataSet.CreateDbConnection())
            {
                DERAnalyticResult result =  new TableOperations<DERAnalyticResult>(connection).QueryRecordWhere("ChannelID = {0} AND Time BETWEEN {1} AND {2}", channelID, time.AddMinutes(-1), time.AddMinutes(1));

                if (result == null) result = new DERAnalyticResult() { 
                    AssetID = assetID, 
                    ChannelID = channelID,
                    MeterID = meterID,
                    EventID = eventID, 
                    DataType = "Event",
                    Parameter = parameter,
                    Threshold = threshold,
                    Regulation = "7.4.2 Limitation of cumulative instantaneous overvoltage",
                    Time = time,
                    Value = value
                };

                new TableOperations<DERAnalyticResult>(connection).AddNewOrUpdateRecord(result);
            }
        }


    }
}
