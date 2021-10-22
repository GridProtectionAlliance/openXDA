//******************************************************************************************************
//  XMLConfigProducer.cs - Gbtc
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
//  10/22/2021 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace openXDA.XMLConfigLoader
{
    public class XMLConfigProducer
    {
        #region [ Static ]
        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(XMLConfigProducer));
        #endregion

        #region [ Members ]
        string SQL = @"
		SELECT 
			Location.LocationKey as [@LocationKey],
			Location.Name as [@Name],
			CAST(Location.Latitude as nvarchar(max)) as [@Latitude],
			CAST(Location.Longitude as nvarchar(max)) as [@Longitude],
			(
				SELECT (
					SELECT 
						Meter.AssetKey as [@AssetKey],
						Meter.Name  as [@Name],
						Meter.Make  as [@Make],
						Meter.Model as [@Model],
						(
							SELECT (
								SELECT (
									SELECT  
											Line.AssetKey as [@AssetKey],
											Line.AssetName as [@AssetName],
											Line.Description as [@Description],
											CAST(Line.VoltageKV as nvarchar(max)) as [@VoltageKV],
											Line.Spare as [@Spare],
											CAST(Line.MaxFaultDistance as nvarchar(max)) as [@MaxFaultDistance],
											CAST(Line.MinFaultDistance as nvarchar(max)) as [@MinFaultDistance],
											(
												SELECT (
													SELECT 
														LineSegment.AssetKey as [@AssetKey],
														LineSegment.AssetName as [@AssetName],
														LineSegment.Description as [@Description],
														CAST(LineSegment.VoltageKV as varchar(max)) as [@VoltageKV],
														CAST(LineSegment.Length as varchar(max)) as [@Length],
														LineSegment.IsEnd as [@IsEnd],
														CAST(LineSegment.ThermalRating as varchar(max)) as [@ThermalRating],
														CAST(LineSegment.R0 as varchar(max)) as [@R0],
														CAST(LineSegment.R1 as varchar(max)) as [@R1],
														CAST(LineSegment.X0 as varchar(max)) as [@X0],
														CAST(LineSegment.X1 as varchar(max)) as [@X1],
														( SELECT (
															SELECT DISTINCT
																Child.AssetKey as [@Key]
															FROM LineSegmentConnections JOIN
																LineSegment Child ON Child.ID = LineSegmentConnections.ChildSegment
															WHERE LineSegmentConnections.ParentSegment = LineSegment.ID FOR XML PATH('ChildSegment'),TYPE
														) FOR XML PATH (''), TYPE),
														( SELECT (
															SELECT DISTINCT
																EndStation.LocationKey as [@LocationKey]
															FROM AssetLocation JOIN
																Location EndStation ON AssetLocation.LocationID = EndStation.ID
															WHERE AssetLocation.AssetID = LineSegment.ID AND EndStation.ID != Location.ID FOR XML PATH('Station'),TYPE
														) FOR XML PATH (''), TYPE)
													FROM LineSegment JOIN
														 AssetConnectionDetail ON AssetConnectionDetail.ChildID = LineSegment.ID 
													WHERE AssetConnectionDetail.ParentID = Line.ID  FOR XML PATH('Segment'),TYPE
											) FOR XML PATH(''), TYPE ),
											( SELECT (
												SELECT DISTINCT
													EndStation.LocationKey as [@Key]
												FROM AssetLocation JOIN
													Location EndStation ON AssetLocation.LocationID = EndStation.ID
												WHERE AssetLocation.AssetID = Line.ID AND EndStation.ID != Location.ID FOR XML PATH('Station'),TYPE
											) FOR XML PATH (''), TYPE),

											( SELECT (
												SELECT DISTINCT
													AssetConnectionDetail.AssetRelationshipType as [@Type],
													AssetConnectionDetail.ChildKey as [@Child] 
												FROM 
													AssetConnectionDetail 							
												WHERE AssetConnectionDetail.ParentID = Line.ID  AND AssetConnectionDetail.AssetRelationshipType != 'Line-LineSegment' FOR XML PATH('Connection'),TYPE
											) FOR XML PATH (''), TYPE),
											( SELECT (
												SELECT 
													ChannelDetail.MeasurementType as [@MeasurementType],
													ChannelDetail.MeasurementCharacteristic as [@MeasurementCharacteristic],
													ChannelDetail.Phase as [@Phase], 
													ChannelDetail.Name as [@Name],
													ChannelDetail.HarmonicGroup as [@HarmonicGroup],
													ChannelDetail.Description as [@Description],
													ChannelDetail.Enabled as [@Enabled],
													CAST(ChannelDetail.PerUnitValue as varchar(max)) as [@PerUnitValue],
													CAST(ChannelDetail.SamplesPerHour as varchar(max)) as [@SamplesPerHour],
													CAST(ChannelDetail.Adder as varchar(max)) as [@Adder],
													ChannelDetail.ConnectionPriority as [@ConnectionPriority],
													CAST(ChannelDetail.Multiplier as varchar(max)) as [@Multiplier],
													(
														SELECT (SELECT SeriesType.Name as [@Type], Series.SourceIndexes as [@SourceIndexes] FROM Series JOIN SeriesType ON Series.SeriesTypeID = SeriesType.ID WHERE Series.ChannelID = ChannelDetail.ID FOR XML PATH('Series'),TYPE ) FOR XML PATH(''), TYPE
													) 
												FROM ChannelDetail 								
												WHERE ChannelDetail.MeterID = Meter.ID AND ChannelDetail.AssetID = Line.ID FOR XML PATH('Channel'),TYPE
											) FOR XML PATH (''), TYPE)  
										FROM MeterAsset JOIN 
											Line ON MeterAsset.AssetID = Line.ID 
										WHERE MeterAsset.MeterID = Meter.ID FOR XML PATH('Line'),TYPE 
								) FOR XML PATH(''), TYPE
							) ,
							(
								SELECT (
									SELECT  
										Bus.AssetKey as [@AssetKey],
										Bus.AssetName as [@AssetName],
										Bus.Description as [@Description],
										CAST(Bus.VoltageKV as nvarchar(max)) as [@VoltageKV],
										Bus.Spare as [@Spare],
										( SELECT (
											SELECT DISTINCT
												EndStation.LocationKey as [@Key]
											FROM AssetLocation JOIN
												Location EndStation ON AssetLocation.LocationID = EndStation.ID
											WHERE AssetLocation.AssetID = Bus.ID AND EndStation.ID != Location.ID FOR XML PATH('Station'),TYPE
										) FOR XML PATH (''), TYPE),


										( SELECT (
											SELECT DISTINCT
												AssetConnectionDetail.AssetRelationshipType as [@Type],
												AssetConnectionDetail.ChildKey as [@Child] 
											FROM 
												AssetConnectionDetail 							
											WHERE AssetConnectionDetail.ParentID = Bus.ID FOR XML PATH('Connection'),TYPE
										) FOR XML PATH (''), TYPE),
										( SELECT (
											SELECT 
												ChannelDetail.MeasurementType as [@MeasurementType],
												ChannelDetail.MeasurementCharacteristic as [@MeasurementCharacteristic],
												ChannelDetail.Phase as [@Phase], 
												ChannelDetail.Name as [@Name],
												ChannelDetail.HarmonicGroup as [@HarmonicGroup],
												ChannelDetail.Description as [@Description],
												ChannelDetail.Enabled as [@Enabled],
												CAST(ChannelDetail.PerUnitValue as varchar(max)) as [@PerUnitValue],
												CAST(ChannelDetail.SamplesPerHour as varchar(max)) as [@SamplesPerHour],
												CAST(ChannelDetail.Adder as varchar(max)) as [@Adder],
												ChannelDetail.ConnectionPriority as [@ConnectionPriority],
												CAST(ChannelDetail.Multiplier as varchar(max)) as [@Multiplier],
												(
													SELECT (SELECT SeriesType.Name as [@Type], Series.SourceIndexes as [@SourceIndexes] FROM Series JOIN SeriesType ON Series.SeriesTypeID = SeriesType.ID WHERE Series.ChannelID = ChannelDetail.ID FOR XML PATH('Series'),TYPE ) FOR XML PATH(''), TYPE
												) 
											FROM ChannelDetail 								
											WHERE ChannelDetail.MeterID = Meter.ID AND ChannelDetail.AssetID = Bus.ID FOR XML PATH('Channel'),TYPE
										) FOR XML PATH (''), TYPE)  
									FROM MeterAsset JOIN 
										Bus ON MeterAsset.AssetID = Bus.ID
									WHERE MeterAsset.MeterID = Meter.ID FOR XML PATH('Bus'),TYPE 
								) FOR XML PATH(''), TYPE
								),
								(
									SELECT (
										SELECT  
											Breaker.AssetKey as [@AssetKey],
											Breaker.AssetName as [@AssetName],
											Breaker.Description as [@Description],
											CAST(Breaker.VoltageKV as nvarchar(max)) as [@VoltageKV],
											Breaker.Spare as [@Spare],
											CAST(Breaker.PickupTime as nvarchar(max)) as [@PickupTime],
											CAST(Breaker.Speed as nvarchar(max)) as [@Speed],
											CAST(Breaker.ThermalRating as nvarchar(max)) as [@ThermalRating],
											CAST(Breaker.TripTime as nvarchar(max)) as [@TripTime],
											( SELECT (
												SELECT DISTINCT
													EndStation.LocationKey as [@Key]
												FROM AssetLocation JOIN
													Location EndStation ON AssetLocation.LocationID = EndStation.ID
												WHERE AssetLocation.AssetID = Breaker.ID AND EndStation.ID != Location.ID FOR XML PATH('Station'),TYPE
											) FOR XML PATH (''), TYPE),

											( SELECT (
												SELECT DISTINCT
													AssetConnectionDetail.AssetRelationshipType as [@Type],
													AssetConnectionDetail.ChildKey as [@Child] 
												FROM 
													AssetConnectionDetail 							
												WHERE AssetConnectionDetail.ParentID = Breaker.ID FOR XML PATH('Connection'),TYPE
											) FOR XML PATH (''), TYPE),
										( SELECT (
											SELECT 
												ChannelDetail.MeasurementType as [@MeasurementType],
												ChannelDetail.MeasurementCharacteristic as [@MeasurementCharacteristic],
												ChannelDetail.Phase as [@Phase], 
												ChannelDetail.Name as [@Name],
												ChannelDetail.HarmonicGroup as [@HarmonicGroup],
												ChannelDetail.Description as [@Description],
												ChannelDetail.Enabled as [@Enabled],
												CAST(ChannelDetail.PerUnitValue as varchar(max)) as [@PerUnitValue],
												CAST(ChannelDetail.SamplesPerHour as varchar(max)) as [@SamplesPerHour],
												CAST(ChannelDetail.Adder as varchar(max)) as [@Adder],
												ChannelDetail.ConnectionPriority as [@ConnectionPriority],
												CAST(ChannelDetail.Multiplier as varchar(max)) as [@Multiplier],
												(
													SELECT (SELECT SeriesType.Name as [@Type], Series.SourceIndexes as [@SourceIndexes] FROM Series JOIN SeriesType ON Series.SeriesTypeID = SeriesType.ID WHERE Series.ChannelID = ChannelDetail.ID FOR XML PATH('Series'),TYPE ) FOR XML PATH(''), TYPE
												) 
											FROM ChannelDetail 								
											WHERE ChannelDetail.MeterID = Meter.ID AND ChannelDetail.AssetID = Breaker.ID FOR XML PATH('Channel'),TYPE
										) FOR XML PATH (''), TYPE) 
										FROM MeterAsset JOIN 
											Breaker ON MeterAsset.AssetID = Breaker.ID
										WHERE MeterAsset.MeterID = Meter.ID FOR XML PATH('Breaker'),TYPE 
									) FOR XML PATH(''), TYPE	
								),
								(
									SELECT (
										SELECT  
											CapBank.AssetKey as [@AssetKey],
											CapBank.AssetName as [@AssetName],
											CapBank.Description as [@Description],
											CAST(CapBank.VoltageKV as nvarchar(max)) as [@VoltageKV],
											CapBank.Spare as [@Spare],
											CAST(CapBank.BlownFuses as nvarchar(max)) as [@BlownFuses],
											CAST(CapBank.BlownGroups as nvarchar(max)) as [@BlownGroups],
											CAST(CapBank.CapacitancePerBank as nvarchar(max)) as [@CapacitancePerBank],
											CapBank.CktSwitcher as [@CktSwitcher],
											CAST(CapBank.Compensated as nvarchar(max)) as [@Compensated],
											CAST(CapBank.Fused as nvarchar(max)) as [@Fused],
											CAST(CapBank.LowerXFRRatio as nvarchar(max)) as [@LowerXFRRatio],
											CAST(CapBank.LVKV as nvarchar(max)) as [@LVKV],
											CAST(CapBank.LVKVAr as nvarchar(max)) as [@LVKVAr],
											CAST(CapBank.LVNegReactanceTol as nvarchar(max)) as [@LVNegReactanceTol],
											CAST(CapBank.LVPosReactanceTol as nvarchar(max)) as [@LVPosReactanceTol],
											CAST(CapBank.MaxKV as nvarchar(max)) as [@MaxKV],
											CAST(CapBank.NegReactanceTol as nvarchar(max)) as [@NegReactanceTol],
											CAST(CapBank.NLowerGroups as nvarchar(max)) as [@NLowerGroups],
											CAST(CapBank.Nparalell as nvarchar(max)) as [@Nparalell],
											CAST(CapBank.NParalellGroup as nvarchar(max)) as [@NParalellGroup],
											CAST(CapBank.Nseries as nvarchar(max)) as [@Nseries],
											CAST(CapBank.NSeriesGroup as nvarchar(max)) as [@NSeriesGroup],
											CAST(CapBank.Nshorted as nvarchar(max)) as [@Nshorted],
											CAST(CapBank.NumberLVCaps as nvarchar(max)) as [@NumberLVCaps],
											CAST(CapBank.NumberLVUnits as nvarchar(max)) as [@NumberLVUnits],
											CAST(CapBank.NumberOfBanks as nvarchar(max)) as [@NumberOfBanks],
											CAST(CapBank.PosReactanceTol as nvarchar(max)) as [@PosReactanceTol],
											CAST(CapBank.RelayPTRatioPrimary as nvarchar(max)) as [@RelayPTRatioPrimary],
											CAST(CapBank.RelayPTRatioSecondary as nvarchar(max)) as [@RelayPTRatioSecondary],
											CAST(CapBank.Rh as nvarchar(max)) as [@Rh],
											CAST(CapBank.Rv as nvarchar(max)) as [@Rv],
											CAST(CapBank.Sh as nvarchar(max)) as [@Sh],
											CAST(CapBank.ShortedGroups as nvarchar(max)) as [@ShortedGroups],
											CAST(CapBank.UnitKV as nvarchar(max)) as [@UnitKV],
											CAST(CapBank.UnitKVAr as nvarchar(max)) as [@UnitKVAr],
											CAST(CapBank.VTratioBus as nvarchar(max)) as [@VTratioBus],
											( SELECT (
												SELECT DISTINCT
													EndStation.LocationKey as [@Key]
												FROM AssetLocation JOIN
													Location EndStation ON AssetLocation.LocationID = EndStation.ID
												WHERE AssetLocation.AssetID = CapBank.ID AND EndStation.ID != Location.ID FOR XML PATH('Station'),TYPE
											) FOR XML PATH (''), TYPE),

											( SELECT (
												SELECT DISTINCT
													AssetConnectionDetail.AssetRelationshipType as [@Type],
													AssetConnectionDetail.ChildKey as [@Child] 
												FROM 
													AssetConnectionDetail 							
												WHERE AssetConnectionDetail.ParentID = CapBank.ID FOR XML PATH('Connection'),TYPE
											) FOR XML PATH (''), TYPE),
										( SELECT (
											SELECT 
												ChannelDetail.MeasurementType as [@MeasurementType],
												ChannelDetail.MeasurementCharacteristic as [@MeasurementCharacteristic],
												ChannelDetail.Phase as [@Phase], 
												ChannelDetail.Name as [@Name],
												ChannelDetail.HarmonicGroup as [@HarmonicGroup],
												ChannelDetail.Description as [@Description],
												ChannelDetail.Enabled as [@Enabled],
												CAST(ChannelDetail.PerUnitValue as varchar(max)) as [@PerUnitValue],
												CAST(ChannelDetail.SamplesPerHour as varchar(max)) as [@SamplesPerHour],
												CAST(ChannelDetail.Adder as varchar(max)) as [@Adder],
												ChannelDetail.ConnectionPriority as [@ConnectionPriority],
												CAST(ChannelDetail.Multiplier as varchar(max)) as [@Multiplier],
												(
													SELECT (SELECT SeriesType.Name as [@Type], Series.SourceIndexes as [@SourceIndexes] FROM Series JOIN SeriesType ON Series.SeriesTypeID = SeriesType.ID WHERE Series.ChannelID = ChannelDetail.ID FOR XML PATH('Series'),TYPE ) FOR XML PATH(''), TYPE
												) 
											FROM ChannelDetail 								
											WHERE ChannelDetail.MeterID = Meter.ID AND ChannelDetail.AssetID = CapBank.ID FOR XML PATH('Channel'),TYPE
										) FOR XML PATH (''), TYPE) 
										FROM MeterAsset JOIN 
											CapBank ON MeterAsset.AssetID = CapBank.ID
										WHERE MeterAsset.MeterID = Meter.ID FOR XML PATH('CapacitorBank'),TYPE 
									) FOR XML PATH(''), TYPE	
								),
								(
									SELECT (
										SELECT 
											Transformer.AssetKey as [@AssetKey],
											Transformer.AssetName as [@AssetName],
											Transformer.Description as [@Description],
											CAST(Transformer.VoltageKV as nvarchar(max)) as [@VoltageKV],
											Transformer.Spare as [@Spare],
											CAST(Transformer.PrimaryVoltageKV as nvarchar(max)) as [@PrimaryVoltageKV],
											CAST(Transformer.PrimaryWinding as nvarchar(max)) as [@PrimaryWinding],
											CAST(Transformer.SecondaryVoltageKV as nvarchar(max)) as [@SecondaryVoltageKV],
											CAST(Transformer.SecondaryWinding as nvarchar(max)) as [@SecondaryWinding],
											CAST(Transformer.TertiaryVoltageKV as nvarchar(max)) as [@TertiaryVoltageKV],
											CAST(Transformer.TertiaryWinding as nvarchar(max)) as [@TertiaryWinding],
											CAST(Transformer.R0 as varchar(max)) as [@R0],
											CAST(Transformer.R1 as varchar(max)) as [@R1],
											CAST(Transformer.X0 as varchar(max)) as [@X0],
											CAST(Transformer.X1 as varchar(max)) as [@X1],
											CAST(Transformer.TAP as varchar(max)) as [@Tap],
											CAST(Transformer.ThermalRating as varchar(max)) as [@ThermalRating],
											( SELECT (
												SELECT DISTINCT
													EndStation.LocationKey as [@Key]
												FROM AssetLocation JOIN
													Location EndStation ON AssetLocation.LocationID = EndStation.ID
												WHERE AssetLocation.AssetID = Transformer.ID AND EndStation.ID != Location.ID FOR XML PATH('Station'),TYPE
											) FOR XML PATH (''), TYPE),

											( SELECT (
												SELECT DISTINCT
													AssetConnectionDetail.AssetRelationshipType as [@Type],
													AssetConnectionDetail.ChildKey as [@Child] 
												FROM 
													AssetConnectionDetail 							
												WHERE AssetConnectionDetail.ParentID = Transformer.ID FOR XML PATH('Connection'),TYPE
											) FOR XML PATH (''), TYPE),
										( SELECT (
											SELECT 
												ChannelDetail.MeasurementType as [@MeasurementType],
												ChannelDetail.MeasurementCharacteristic as [@MeasurementCharacteristic],
												ChannelDetail.Phase as [@Phase], 
												ChannelDetail.Name as [@Name],
												ChannelDetail.HarmonicGroup as [@HarmonicGroup],
												ChannelDetail.Description as [@Description],
												ChannelDetail.Enabled as [@Enabled],
												CAST(ChannelDetail.PerUnitValue as varchar(max)) as [@PerUnitValue],
												CAST(ChannelDetail.SamplesPerHour as varchar(max)) as [@SamplesPerHour],
												CAST(ChannelDetail.Adder as varchar(max)) as [@Adder],
												ChannelDetail.ConnectionPriority as [@ConnectionPriority],
												CAST(ChannelDetail.Multiplier as varchar(max)) as [@Multiplier],
												(
													SELECT (SELECT SeriesType.Name as [@Type], Series.SourceIndexes as [@SourceIndexes] FROM Series JOIN SeriesType ON Series.SeriesTypeID = SeriesType.ID WHERE Series.ChannelID = ChannelDetail.ID FOR XML PATH('Series'),TYPE ) FOR XML PATH(''), TYPE
												) 
											FROM ChannelDetail 								
											WHERE ChannelDetail.MeterID = Meter.ID AND ChannelDetail.AssetID = Transformer.ID FOR XML PATH('Channel'),TYPE
										) FOR XML PATH (''), TYPE) 
										FROM MeterAsset JOIN 
											Transformer ON MeterAsset.AssetID = Transformer.ID
										WHERE MeterAsset.MeterID = Meter.ID FOR XML PATH('Transformer'),TYPE 
									) FOR XML PATH(''), TYPE	
							) ,
							(
								SELECT (
									SELECT  
										CapBankRelay.AssetKey as [@AssetKey],
										CapBankRelay.AssetName as [@AssetName],
										CapBankRelay.Description as [@Description],
										CAST(CapBankRelay.VoltageKV as nvarchar(max)) as [@VoltageKV],
										CapBankRelay.Spare as [@Spare],
										CAST(CapBankRelay.CapBankNumber as nvarchar(max)) as [@CapBankNumber],
										CAST(CapBankRelay.OnVoltageThreshhold as nvarchar(max)) as [@OnVoltageThreshhold],
										( SELECT (
											SELECT DISTINCT
												EndStation.LocationKey as [@Key]
											FROM AssetLocation JOIN
												Location EndStation ON AssetLocation.LocationID = EndStation.ID
											WHERE AssetLocation.AssetID = CapBankRelay.ID AND EndStation.ID != Location.ID FOR XML PATH('Station'),TYPE
										) FOR XML PATH (''), TYPE),

										( SELECT (
											SELECT DISTINCT
												AssetConnectionDetail.AssetRelationshipType as [@Type],
												AssetConnectionDetail.ChildKey as [@Child] 
											FROM 
												AssetConnectionDetail 							
											WHERE AssetConnectionDetail.ParentID = CapBankRelay.ID FOR XML PATH('Connection'),TYPE
										) FOR XML PATH (''), TYPE),
										( SELECT (
											SELECT 
												ChannelDetail.MeasurementType as [@MeasurementType],
												ChannelDetail.MeasurementCharacteristic as [@MeasurementCharacteristic],
												ChannelDetail.Phase as [@Phase], 
												ChannelDetail.Name as [@Name],
												ChannelDetail.HarmonicGroup as [@HarmonicGroup],
												ChannelDetail.Description as [@Description],
												ChannelDetail.Enabled as [@Enabled],
												CAST(ChannelDetail.PerUnitValue as varchar(max)) as [@PerUnitValue],
												CAST(ChannelDetail.SamplesPerHour as varchar(max)) as [@SamplesPerHour],
												CAST(ChannelDetail.Adder as varchar(max)) as [@Adder],
												ChannelDetail.ConnectionPriority as [@ConnectionPriority],
												CAST(ChannelDetail.Multiplier as varchar(max)) as [@Multiplier],
												(
													SELECT (SELECT SeriesType.Name as [@Type], Series.SourceIndexes as [@SourceIndexes] FROM Series JOIN SeriesType ON Series.SeriesTypeID = SeriesType.ID WHERE Series.ChannelID = ChannelDetail.ID FOR XML PATH('Series'),TYPE ) FOR XML PATH(''), TYPE
												) 
											FROM ChannelDetail 								
											WHERE ChannelDetail.MeterID = Meter.ID AND ChannelDetail.AssetID = CapBankRelay.ID FOR XML PATH('Channel'),TYPE
										) FOR XML PATH (''), TYPE) 
									FROM MeterAsset JOIN 
										CapBankRelay ON MeterAsset.AssetID = CapBankRelay.ID
									WHERE MeterAsset.MeterID = Meter.ID FOR XML PATH('CapacitorBankRelay'),TYPE 
								) FOR XML PATH(''), TYPE
							) ,
							(
								SELECT (
									SELECT  
										DER.AssetKey as [@AssetKey],
										DER.AssetName as [@AssetName],
										DER.Description as [@Description],
										CAST(DER.VoltageKV as nvarchar(max)) as [@VoltageKV],
										DER.Spare as [@Spare],
										CAST(DER.FullRatedOutputCurrent as nvarchar(max)) as [@FullRatedOutputCurrent],
										DER.VoltageLevel as [@VoltageLevel],
										( SELECT (
											SELECT DISTINCT
												EndStation.LocationKey as [@Key]
											FROM AssetLocation JOIN
												Location EndStation ON AssetLocation.LocationID = EndStation.ID
											WHERE AssetLocation.AssetID = DER.ID AND EndStation.ID != Location.ID FOR XML PATH('Station'),TYPE
										) FOR XML PATH (''), TYPE),

										( SELECT (
											SELECT DISTINCT
												AssetConnectionDetail.AssetRelationshipType as [@Type],
												AssetConnectionDetail.ChildKey as [@Child] 
											FROM 
												AssetConnectionDetail 							
											WHERE AssetConnectionDetail.ParentID = DER.ID FOR XML PATH('Connection'),TYPE
										) FOR XML PATH (''), TYPE),
										( SELECT (
											SELECT 
												ChannelDetail.MeasurementType as [@MeasurementType],
												ChannelDetail.MeasurementCharacteristic as [@MeasurementCharacteristic],
												ChannelDetail.Phase as [@Phase], 
												ChannelDetail.Name as [@Name],
												ChannelDetail.HarmonicGroup as [@HarmonicGroup],
												ChannelDetail.Description as [@Description],
												ChannelDetail.Enabled as [@Enabled],
												CAST(ChannelDetail.PerUnitValue as varchar(max)) as [@PerUnitValue],
												CAST(ChannelDetail.SamplesPerHour as varchar(max)) as [@SamplesPerHour],
												CAST(ChannelDetail.Adder as varchar(max)) as [@Adder],
												ChannelDetail.ConnectionPriority as [@ConnectionPriority],
												CAST(ChannelDetail.Multiplier as varchar(max)) as [@Multiplier],
												(
													SELECT (SELECT SeriesType.Name as [@Type], Series.SourceIndexes as [@SourceIndexes] FROM Series JOIN SeriesType ON Series.SeriesTypeID = SeriesType.ID WHERE Series.ChannelID = ChannelDetail.ID FOR XML PATH('Series'),TYPE ) FOR XML PATH(''), TYPE
												) 
											FROM ChannelDetail 								
											WHERE ChannelDetail.MeterID = Meter.ID AND ChannelDetail.AssetID = DER.ID FOR XML PATH('Channel'),TYPE
										) FOR XML PATH (''), TYPE) 
									FROM MeterAsset JOIN 
										DER ON MeterAsset.AssetID = DER.ID
									WHERE MeterAsset.MeterID = Meter.ID ##ASSET_QUERY##
									FOR XML PATH('DER'),TYPE 
								) FOR XML PATH(''), TYPE

							) FOR XML PATH(''), TYPE	
										
						)
					FROM 
						Meter 
					WHERE Meter.LocationID = Location.ID ##METER_QUERY##
					FOR XML PATH('Device'), TYPE
		
				)
				FOR XML PATH(''), TYPE
			)
		FROM 
			Location 
		##LOCATION_QUERY##
		FOR XML PATH('Station'), ROOT('OpenXDA')

        ";
        #endregion

        #region [ Properties ]
        private string ConnectionString { get; set; }
        private string DataProvider { get; set; }
        #endregion

        #region [ Constructor ]
		/// <summary>
		/// Instantiates XMLConfigProducer and points it at specific database
		/// </summary>
		/// <param name="connectionString">Database connection string</param>
		/// <param name="dataProvider">Data provider dll information</param>
        public XMLConfigProducer(string connectionString, string dataProvider)
        {
            ConnectionString = connectionString;
            DataProvider = dataProvider;
        }
        #endregion

        #region [ Methods ]
		/// <summary>
		/// Get XML and returns ASCII encoded string in a MemoryStream
		/// </summary>
		/// <param name="locationIDs"></param>
		/// <param name="meterIDs"></param>
		/// <param name="assetIDs"></param>
		/// <returns></returns>
        public MemoryStream Get(List<int> locationIDs = null, List<int> meterIDs = null, List<int> assetIDs = null)
        {
			string sql = BuildQuery(locationIDs, meterIDs, assetIDs);
			string xml = Query(sql);
			MemoryStream stream = new MemoryStream();
			byte[] bytes = Encoding.ASCII.GetBytes(xml);
			stream.Write(bytes, 0, bytes.Length);
			return stream;
		}

		/// <summary>
		/// Gets XML and writes a file to supplied file path
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="locationIDs"></param>
		/// <param name="meterIDs"></param>
		/// <param name="assetIDs"></param>
		public void Get(string filePath, List<int> locationIDs=null, List<int> meterIDs=null, List<int> assetIDs=null)
		{
			string sql = BuildQuery(locationIDs, meterIDs, assetIDs);
			string xml = Query(sql);
			File.WriteAllText(filePath, xml);
		}

		/// <summary>
		/// Gets XML and returns as string
		/// </summary>
		/// <param name="locationIDs"></param>
		/// <param name="meterIDs"></param>
		/// <param name="assetIDs"></param>
		/// <returns></returns>
		public string ToString(List<int> locationIDs = null, List<int> meterIDs = null, List<int> assetIDs = null)
		{
			string sql = BuildQuery(locationIDs, meterIDs, assetIDs);
			string xml = Query(sql);
			return xml;
		}



		private string BuildQuery(List<int> locationIDs, List<int> meterIDs, List<int> assetIDs)
        {
			string sql = SQL;
			if (locationIDs != null || locationIDs?.Count > 0)
			{
				sql = sql.Replace("##LOCATION_QUERY##", $"WHERE Location.ID IN ({string.Join(",", locationIDs)})");
			}
			else
				sql = sql.Replace("##LOCATION_QUERY##", "");

			if (meterIDs != null || meterIDs?.Count > 0)
			{
				sql = sql.Replace("##METER_QUERY##", $"WHERE Meter.ID IN ({string.Join(",", meterIDs)})");
			}
			else
				sql = sql.Replace("##METER_QUERY##", "");


			if (assetIDs != null || assetIDs?.Count > 0)
			{
				sql = sql.Replace("##ASSET_QUERY##", $"WHERE Asset.ID IN ({string.Join(",", assetIDs)})");
			}
			else
				sql = sql.Replace("##ASSET_QUERY##", "");

			return sql;
		}

		private string Query(string sql) {
			StringWriter stringWriter = new StringWriter();

			using (AdoDataConnection connection = new AdoDataConnection(ConnectionString, DataProvider))
			{
				var reader = connection.ExecuteReader(sql);
				string xml = "";
				while (reader.Read())
					xml += string.Format("{0}", reader[0]);

				XmlDocument document = new XmlDocument();
				document.LoadXml(xml);
				document.Save(stringWriter);
				return stringWriter.ToString();
			}

		}
		#endregion
	}
}
