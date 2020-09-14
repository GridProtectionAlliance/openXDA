//******************************************************************************************************
//  LazyContext.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  09/04/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using Gemstone.Data;

namespace OpenXDA.Model
{
    internal class LazyContext
    {
        #region [ Members ]

        // Fields
        private Dictionary<int, Location> m_locations;
        private Dictionary<int, Meter> m_meters;
        private Dictionary<int, AssetLocation> m_assetLocations;
        private Dictionary<int, SourceImpedance> m_sourceImpedances;
        private Dictionary<int, MeterAsset> m_meterAssets;
        private Dictionary<int, Channel> m_channels;
        private Dictionary<int, Series> m_series;
        private Dictionary<int, MeasurementType> m_measurementTypes;
        private Dictionary<int, MeasurementCharacteristic> m_measurementCharacteristics;
        private Dictionary<int, Phase> m_phases;
        private Dictionary<int, SeriesType> m_seriesTypes;

        private Dictionary<int, Asset> m_assets;
        private Dictionary<int, AssetConnection> m_assetConnections;
        private Dictionary<int, LineSegmentConnections> m_segmentConnections;
        private Dictionary<int, Line> m_lines;
        private Dictionary<int, LineSegment> m_lineSegments;

        #endregion

        #region [ Constructors ]

        public LazyContext()
        {
            m_locations = new Dictionary<int, Location>();
            m_meters = new Dictionary<int, Meter>();
            m_assets = new Dictionary<int, Asset>();
            m_assetLocations = new Dictionary<int, AssetLocation>();
            m_sourceImpedances = new Dictionary<int, SourceImpedance>();
            m_meterAssets = new Dictionary<int, MeterAsset>();
            m_channels = new Dictionary<int, Channel>();
            m_series = new Dictionary<int, Series>();
            m_measurementTypes = new Dictionary<int, MeasurementType>();
            m_measurementCharacteristics = new Dictionary<int, MeasurementCharacteristic>();
            m_phases = new Dictionary<int, Phase>();
            m_seriesTypes = new Dictionary<int, SeriesType>();
            m_assetConnections = new Dictionary<int, AssetConnection>();
            m_lines = new Dictionary<int, Line>();
            m_lineSegments = new Dictionary<int, LineSegment>();
            m_segmentConnections = new Dictionary<int, LineSegmentConnections>();
        }

        #endregion

        #region [ Properties ]

        public Func<AdoDataConnection> ConnectionFactory { get; set; }

        #endregion

        #region [ Methods ]

        public Location GetLocation(Location location)
        {
            Location cachedLocation;

            if ((object)location == null)
                return null;

            if (location.ID == 0)
                return location;

            if (m_locations.TryGetValue(location.ID, out cachedLocation))
                return cachedLocation;

            m_locations.Add(location.ID, location);
            return location;
        }

        public Meter GetMeter(Meter meter)
        {
            Meter cachedMeter;

            if ((object)meter == null)
                return null;

            if (meter.ID == 0)
                return meter;

            if (m_meters.TryGetValue(meter.ID, out cachedMeter))
                return cachedMeter;

            m_meters.Add(meter.ID, meter);
            return meter;
        }

        public Asset GetAsset(Asset asset)
        {
            Asset cachedAsset;

            if ((object)asset == null)
                return null;

            if (asset.ID == 0)
                return asset;

            if (m_assets.TryGetValue(asset.ID, out cachedAsset))
                return cachedAsset;

            m_assets.Add(asset.ID, asset);
            return asset;
        }

        public AssetLocation GetAssetLocation(AssetLocation assetLocation)
        {
            AssetLocation cachedAssetLocation;

            if ((object)assetLocation == null)
                return null;

            if (assetLocation.ID == 0)
                return assetLocation;

            if (m_assetLocations.TryGetValue(assetLocation.ID, out cachedAssetLocation))
                return cachedAssetLocation;

            m_assetLocations.Add(assetLocation.ID, assetLocation);
            return assetLocation;
        }

        public SourceImpedance GetSourceImpedance(SourceImpedance sourceImpedance)
        {
            SourceImpedance cachedSourceImpedance;

            if ((object)sourceImpedance == null)
                return null;

            if (sourceImpedance.ID == 0)
                return sourceImpedance;

            if (m_sourceImpedances.TryGetValue(sourceImpedance.ID, out cachedSourceImpedance))
                return cachedSourceImpedance;

            m_sourceImpedances.Add(sourceImpedance.ID, sourceImpedance);
            return sourceImpedance;
        }

        public MeterAsset GetMeterAsset(MeterAsset meterAsset)
        {
            MeterAsset cachedMeterAsset;

            if ((object)meterAsset == null)
                return null;

            if (meterAsset.ID == 0)
                return meterAsset;

            if (m_meterAssets.TryGetValue(meterAsset.ID, out cachedMeterAsset))
                return cachedMeterAsset;

            m_meterAssets.Add(meterAsset.ID, meterAsset);
            return meterAsset;
        }

        public Channel GetChannel(Channel channel)
        {
            Channel cachedChannelLine;

            if ((object)channel == null)
                return null;

            if (channel.ID == 0)
                return channel;

            if (m_channels.TryGetValue(channel.ID, out cachedChannelLine))
                return cachedChannelLine;

            m_channels.Add(channel.ID, channel);
            return channel;
        }

        public Series GetSeries(Series series)
        {
            Series cachedSeriesLine;

            if ((object)series == null)
                return null;

            if (series.ID == 0)
                return series;

            if (m_series.TryGetValue(series.ID, out cachedSeriesLine))
                return cachedSeriesLine;

            m_series.Add(series.ID, series);
            return series;
        }

        public MeasurementType GetMeasurementType(MeasurementType measurementType)
        {
            MeasurementType cachedMeasurementTypeLine;

            if ((object)measurementType == null)
                return null;

            if (measurementType.ID == 0)
                return measurementType;

            if (m_measurementTypes.TryGetValue(measurementType.ID, out cachedMeasurementTypeLine))
                return cachedMeasurementTypeLine;

            m_measurementTypes.Add(measurementType.ID, measurementType);
            return measurementType;
        }

        public MeasurementCharacteristic GetMeasurementCharacteristic(MeasurementCharacteristic measurementCharacteristic)
        {
            MeasurementCharacteristic cachedMeasurementCharacteristicLine;

            if ((object)measurementCharacteristic == null)
                return null;

            if (measurementCharacteristic.ID == 0)
                return measurementCharacteristic;

            if (m_measurementCharacteristics.TryGetValue(measurementCharacteristic.ID, out cachedMeasurementCharacteristicLine))
                return cachedMeasurementCharacteristicLine;

            m_measurementCharacteristics.Add(measurementCharacteristic.ID, measurementCharacteristic);
            return measurementCharacteristic;
        }

        public Phase GetPhase(Phase phase)
        {
            Phase cachedPhaseLine;

            if ((object)phase == null)
                return null;

            if (phase.ID == 0)
                return phase;

            if (m_phases.TryGetValue(phase.ID, out cachedPhaseLine))
                return cachedPhaseLine;

            m_phases.Add(phase.ID, phase);
            return phase;
        }

        public SeriesType GetSeriesType(SeriesType seriesType)
        {
            SeriesType cachedSeriesTypeLine;

            if ((object)seriesType == null)
                return null;

            if (seriesType.ID == 0)
                return seriesType;

            if (m_seriesTypes.TryGetValue(seriesType.ID, out cachedSeriesTypeLine))
                return cachedSeriesTypeLine;

            m_seriesTypes.Add(seriesType.ID, seriesType);
            return seriesType;
        }

        public AssetConnection GetAssetConnection(AssetConnection connection)
        {
            AssetConnection cachedConnection;

            if ((object)connection == null)
                return null;

            if (connection.ID == 0)
                return connection;

            if (m_assetConnections.TryGetValue(connection.ID, out cachedConnection))
                return cachedConnection;

            m_assetConnections.Add(connection.ID, connection);
            return connection;
        }

        public LineSegmentConnections GetLineSegmentConnection(LineSegmentConnections connection)
        {
            LineSegmentConnections cachedConnection;

            if ((object)connection == null)
                return null;

            if (connection.ID == 0)
                return connection;

            if (m_segmentConnections.TryGetValue(connection.ID, out cachedConnection))
                return cachedConnection;

            m_segmentConnections.Add(connection.ID, connection);
            return connection;
        }

        public Line GetLine(Line line)
        {
            Line cachedLine;

            if ((object)line == null)
                return null;

            if (line.ID == 0)
                return line;

            if (m_lines.TryGetValue(line.ID, out cachedLine))
                return cachedLine;

            m_lines.Add(line.ID, line);
            return line;
        }

        public LineSegment GetLineSegment(LineSegment line)
        {
            LineSegment cachedLine;

            if ((object)line == null)
                return null;

            if (line.ID == 0)
                return line;

            if (m_lineSegments.TryGetValue(line.ID, out cachedLine))
                return cachedLine;

            m_lineSegments.Add(line.ID, line);
            return line;
        }


        #endregion
    }
}
