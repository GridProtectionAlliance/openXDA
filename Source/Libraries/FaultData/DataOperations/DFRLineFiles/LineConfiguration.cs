//******************************************************************************************************
//  LineConfiguration.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
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
//  06/25/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FaultData.DataSets;
using GSF.COMTRADE;
using GSF.Interop;
using GSF.IO;
using GSF.Parsing;
using log4net;
using openXDA.Model;

namespace FaultData.DataOperations.DFRLineFiles
{
    public class LineConfiguration
    {
        #region [ Members ]

        // Nested Types
        public class LineSection
        {
            #region [ Members ]

            // Fields
            private readonly string m_lineName;
            private readonly string m_lineID;
            private readonly string m_faultLogic;
            private readonly string m_breaker1;
            private readonly string m_breaker2;
            private readonly double m_r1;
            private readonly double m_x1;
            private readonly double m_r0;
            private readonly double m_x0;
            private readonly double m_length;
            private readonly ChannelInfo[] m_channels;
            private readonly DigitalInfo[] m_digitals;

            #endregion

            #region [ Constructors ]

            public LineSection(Schema schema, IniFile iniFile, string sectionName)
            {
                BooleanExpression expression;

                m_lineName = iniFile.GetKeyValue(sectionName, LineNameKey);
                m_lineID = iniFile.GetKeyValue(sectionName, LineIDKey);
                m_faultLogic = iniFile.GetKeyValue(sectionName, FaultLogicKey);
                m_breaker1 = iniFile.GetKeyValue(sectionName, Breaker1Key);
                m_breaker2 = iniFile.GetKeyValue(sectionName, Breaker2Key);
                m_r1 = Convert.ToDouble(iniFile.GetKeyValue(sectionName, R1Key));
                m_x1 = Convert.ToDouble(iniFile.GetKeyValue(sectionName, X1Key));
                m_r0 = Convert.ToDouble(iniFile.GetKeyValue(sectionName, R0Key));
                m_x0 = Convert.ToDouble(iniFile.GetKeyValue(sectionName, X0Key));
                m_length = Convert.ToDouble(iniFile.GetKeyValue(sectionName, LengthKey));

                if (string.IsNullOrEmpty(m_breaker1) && string.IsNullOrEmpty(m_breaker2))
                {
                    m_breaker1 = iniFile.GetKeyValue(sectionName, AltBreaker1Key);
                    m_breaker2 = iniFile.GetKeyValue(sectionName, AltBreaker2Key);
                }

                m_channels = iniFile
                    .GetSectionKeys(sectionName)
                    .Where(key => key.EndsWith("Chan", StringComparison.OrdinalIgnoreCase))
                    .Where(key => !key.Equals("RefChan", StringComparison.OrdinalIgnoreCase))
                    .Where(key => iniFile.GetKeyValue(sectionName, key) != "0")
                    .Select(key => CreateChannelInfo(schema, iniFile, sectionName, key))
                    .Where(channelInfo => (object)channelInfo != null)
                    .ToArray();

                m_digitals = new DigitalInfo[0];

                try
                {
                    if (!string.IsNullOrEmpty(m_faultLogic))
                    {
                        expression = new BooleanExpression(m_faultLogic);

                        m_digitals = expression.Variables
                            .Select(variable => new DigitalInfo(schema, variable.Identifier))
                            .ToArray();
                    }
                }
                catch (Exception ex)
                {
                    string message = $"An error occurred parsing fault detection logic for line {m_lineName}: {ex.Message}";
                    Log.Warn(message, new Exception(message, ex));
                }
            }

            public LineSection(MeterDataSet meterDataSet, IniFile iniFile, string sectionName)
            {
                BooleanExpression expression;

                m_lineName = iniFile.GetKeyValue(sectionName, LineNameKey);
                m_lineID = iniFile.GetKeyValue(sectionName, LineIDKey);
                m_faultLogic = iniFile.GetKeyValue(sectionName, FaultLogicKey);
                m_breaker1 = iniFile.GetKeyValue(sectionName, Breaker1Key);
                m_breaker2 = iniFile.GetKeyValue(sectionName, Breaker2Key);
                m_r1 = Convert.ToDouble(iniFile.GetKeyValue(sectionName, R1Key));
                m_x1 = Convert.ToDouble(iniFile.GetKeyValue(sectionName, X1Key));
                m_r0 = Convert.ToDouble(iniFile.GetKeyValue(sectionName, R0Key));
                m_x0 = Convert.ToDouble(iniFile.GetKeyValue(sectionName, X0Key));
                m_length = Convert.ToDouble(iniFile.GetKeyValue(sectionName, LengthKey));

                if (string.IsNullOrEmpty(m_breaker1) && string.IsNullOrEmpty(m_breaker2))
                {
                    m_breaker1 = iniFile.GetKeyValue(sectionName, AltBreaker1Key);
                    m_breaker2 = iniFile.GetKeyValue(sectionName, AltBreaker2Key);
                }

                m_channels = iniFile
                    .GetSectionKeys(sectionName)
                    .Where(key => key.EndsWith("Chan", StringComparison.OrdinalIgnoreCase))
                    .Where(key => !key.Equals("RefChan", StringComparison.OrdinalIgnoreCase))
                    .Where(key => iniFile.GetKeyValue(sectionName, key) != "0")
                    .Select(key => CreateChannelInfo(meterDataSet, iniFile, sectionName, key))
                    .Where(channelInfo => (object)channelInfo != null)
                    .ToArray();

                m_digitals = new DigitalInfo[0];

                try
                {
                    if (!string.IsNullOrEmpty(m_faultLogic))
                    {
                        expression = new BooleanExpression(m_faultLogic);

                        m_digitals = expression.Variables
                            .Select(variable => CreateDigitalInfo(meterDataSet, variable.Identifier))
                            .Where(digital => (object)digital != null)
                            .ToArray();
                    }
                }
                catch (Exception ex)
                {
                    string message = $"An error occurred parsing fault detection logic for line {m_lineName}: {ex.Message}";
                    Log.Warn(message, new Exception(message, ex));
                }
            }

            #endregion

            #region [ Properties ]

            public string LineName
            {
                get
                {
                    return m_lineName;
                }
            }

            public string LineID
            {
                get
                {
                    return m_lineID;
                }
            }

            public string FaultLogic
            {
                get
                {
                    return m_faultLogic;
                }
            }

            public string Breaker1
            {
                get
                {
                    return m_breaker1;
                }
            }

            public string Breaker2
            {
                get
                {
                    return m_breaker2;
                }
            }

            public double R1
            {
                get
                {
                    return m_r1;
                }
            }

            public double X1
            {
                get
                {
                    return m_x1;
                }
            }

            public double R0
            {
                get
                {
                    return m_r0;
                }
            }

            public double X0
            {
                get
                {
                    return m_x0;
                }
            }

            public double Length
            {
                get
                {
                    return m_length;
                }
            }

            public ChannelInfo[] Channels
            {
                get
                {
                    return m_channels;
                }
            }

            public DigitalInfo[] Digitals
            {
                get
                {
                    return m_digitals;
                }
            }

            #endregion

            #region [ Static ]

            // Static Fields
            private static readonly ILog Log = LogManager.GetLogger(typeof(LineSection));

            // Static Methods
            private static ChannelInfo CreateChannelInfo(Schema schema, IniFile iniFile, string sectionName, string key)
            {
                try
                {
                    return new ChannelInfo(schema, iniFile, sectionName, key);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    return null;
                }
            }

            private static ChannelInfo CreateChannelInfo(MeterDataSet meterDataSet, IniFile iniFile, string sectionName, string key)
            {
                try
                {
                    return new ChannelInfo(meterDataSet, iniFile, sectionName, key);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    return null;
                }
            }

            private static DigitalInfo CreateDigitalInfo(MeterDataSet meterDataSet, string channelName)
            {
                try
                {
                    return new DigitalInfo(meterDataSet, channelName);
                }
                catch (Exception ex)
                {
                    Log.Warn(ex.Message, ex);
                    return null;
                }
            }

            #endregion
        }

        public class ChannelInfo
        {
            #region [ Members ]

            // Fields
            private readonly int m_index;
            private readonly string m_name;
            private readonly string m_designation;
            private readonly string m_measurementType;
            private readonly string m_phase;
            private readonly string m_description;

            #endregion

            #region [ Constructors ]

            public ChannelInfo(Schema schema, IniFile iniFile, string sectionName, string key)
            {
                string designation = GetDesignation(key);
                int index = Convert.ToInt32(iniFile.GetKeyValue(sectionName, key));
                AnalogChannel analog = schema?.AnalogChannels.SingleOrDefault(ch => ch.Index == index);

                if ((object)analog != null)
                {
                    m_name = analog.Name;
                    m_description = analog.CircuitComponent;
                }
                else
                {
                    m_name = key.Replace("Chan", "");
                    m_description = key;
                }

                m_index = index;
                m_designation = designation;
                m_measurementType = GetMeasurementType(designation);
                m_phase = GetPhase(designation);
            }

            public ChannelInfo(MeterDataSet meterDataSet, IniFile iniFile, string sectionName, string key)
            {
                string designation = GetDesignation(key);
                int index = Convert.ToInt32(iniFile.GetKeyValue(sectionName, key));

                Channel analog = (index >= 0 && index < meterDataSet.DataSeries.Count)
                    ? meterDataSet.DataSeries[index].SeriesInfo?.Channel
                    : null;

                m_name = analog?.Name ?? key.Replace("Chan", "");
                m_description = analog?.Description ?? key;
                m_index = index;
                m_designation = designation;
                m_measurementType = GetMeasurementType(designation);
                m_phase = GetPhase(designation);
            }

            #endregion

            #region [ Properties ]

            public int Index
            {
                get
                {
                    return m_index;
                }
            }

            public string Name
            {
                get
                {
                    return m_name;
                }
            }

            public string Designation
            {
                get
                {
                    return m_designation;
                }
            }

            public string MeasurementType
            {
                get
                {
                    return m_measurementType;
                }
            }

            public string Phase
            {
                get
                {
                    return m_phase;
                }
            }

            public string Description
            {
                get
                {
                    return m_description;
                }
            }

            #endregion

            #region [ Static ]

            // Static Methods
            private static string GetDesignation(string key)
            {
                return key.Remove(key.IndexOf("Chan"));
            }

            private static string GetMeasurementType(string channelDesignation)
            {
                if (channelDesignation.StartsWith("V", StringComparison.OrdinalIgnoreCase))
                    return "Voltage";

                if (channelDesignation.StartsWith("I", StringComparison.OrdinalIgnoreCase))
                    return "Current";

                return "Unknown";
            }

            private static string GetPhase(string channelDesignation)
            {
                if (channelDesignation.EndsWith("A", StringComparison.OrdinalIgnoreCase))
                    return "AN";

                if (channelDesignation.EndsWith("B", StringComparison.OrdinalIgnoreCase))
                    return "BN";

                if (channelDesignation.EndsWith("C", StringComparison.OrdinalIgnoreCase))
                    return "CN";

                if (channelDesignation.EndsWith("R", StringComparison.OrdinalIgnoreCase))
                    return "NG";

                if (channelDesignation.EndsWith("G", StringComparison.OrdinalIgnoreCase))
                    return "NG";

                if (channelDesignation.EndsWith("N", StringComparison.OrdinalIgnoreCase))
                    return "RES";

                return "None";
            }

            #endregion
        }

        public class DigitalInfo
        {
            #region [ Members ]

            // Fields
            private readonly int m_index;
            private readonly string m_name;
            private readonly string m_measurementCharacteristic;
            private readonly string m_measurementCharacteristicDescription;
            private readonly string m_description;

            #endregion

            #region [ Constructors ]

            public DigitalInfo(Schema schema, string channelName)
                : this(schema.DigitalChannels.FirstOrDefault(ch => ch.Name.Equals(channelName, StringComparison.OrdinalIgnoreCase)), channelName)
            {
            }

            public DigitalInfo(DigitalChannel digitalChannel)
                : this(digitalChannel, null)
            {
            }

            public DigitalInfo(MeterDataSet meterDataSet, string channelName)
            {
                int index = meterDataSet.Digitals
                    .Select(dataSeries => dataSeries.SeriesInfo?.Channel)
                    .TakeWhile(ch => !channelName.Equals(ch?.Name, StringComparison.OrdinalIgnoreCase))
                    .Count();

                if (index >= meterDataSet.Digitals.Count)
                    throw new InvalidOperationException($"Digital channel for identifier '{channelName}' is missing from source data file.");

                Channel channel = meterDataSet.Digitals[index].SeriesInfo.Channel;

                m_index = index;
                m_name = channel.Name;
                m_description = channel.Description;
                m_measurementCharacteristic = GetMeasurementCharacteristic(m_description);
                m_measurementCharacteristicDescription = GetMeasurementCharacteristicDescription(m_description);
            }

            private DigitalInfo(DigitalChannel digitalChannel, string channelName)
            {
                if ((object)digitalChannel == null)
                    throw new InvalidOperationException($"Digital channel for identifier '{channelName}' is missing from COMTRADE schema file.");

                m_index = digitalChannel.Index;
                m_name = digitalChannel.Name;
                m_description = digitalChannel.CircuitComponent;
                m_measurementCharacteristic = GetMeasurementCharacteristic(m_description);
                m_measurementCharacteristicDescription = GetMeasurementCharacteristicDescription(m_description);
            }

            #endregion

            #region [ Properties ]

            public int Index
            {
                get
                {
                    return m_index;
                }
            }

            public string Name
            {
                get
                {
                    return m_name;
                }
            }

            public string MeasurementCharacteristic
            {
                get
                {
                    return m_measurementCharacteristic;
                }
            }

            public string MeasurementCharacteristicDescription
            {
                get
                {
                    return m_measurementCharacteristicDescription;
                }
            }

            public string Description
            {
                get
                {
                    return m_description;
                }
            }

            #endregion

            #region [ Static ]

            // Static Methods
            private static string GetMeasurementCharacteristic(string channelDescription)
            {
                if (IsTCE(channelDescription))
                    return "TCE";

                if (IsStatus(channelDescription))
                    return "BreakerStatus";

                return "None";
            }

            private static string GetMeasurementCharacteristicDescription(string channelDescription)
            {
                if (IsTCE(channelDescription))
                    return "Trip Coil Energized";

                if (IsStatus(channelDescription))
                    return "Breaker Status";

                return "None";
            }

            private static bool IsTCE(string channelDescription)
            {
                return Regex.IsMatch(channelDescription, "TRIP COIL ENERGIZED", RegexOptions.IgnoreCase) ||
                       Regex.IsMatch(channelDescription, "CONTROL TRIP", RegexOptions.IgnoreCase);
            }

            private static bool IsStatus(string channelDescription)
            {
                return Regex.IsMatch(channelDescription, "STATUS", RegexOptions.IgnoreCase);
            }

            #endregion
        }

        // Constants
        private const string LineNameKey = "LineName";
        private const string LineIDKey = "LineID";
        private const string FaultLogicKey = "LineFaultLogic";
        private const string Breaker1Key = "Breaker1";
        private const string Breaker2Key = "Breaker2";
        private const string AltBreaker1Key = "Breaker1ID";
        private const string AltBreaker2Key = "Breaker2ID";
        private const string R1Key = "PosSeqR";
        private const string X1Key = "PosSeqX";
        private const string R0Key = "ZeroSeqR";
        private const string X0Key = "ZeroSeqX";
        private const string LengthKey = "LineMiles";

        // Fields
        private readonly string m_assetKey;
        private readonly LineSection[] m_lineSections;

        #endregion

        #region [ Constructors ]

        public LineConfiguration(Schema schema, string[] directories, string assetKey)
        {
            IniFile iniFile;
            string fileName;
            string filePath;

            iniFile = null;
            fileName = string.Format("{0}Lines.inf", assetKey);

            foreach (string directory in directories)
            {
                filePath = Directory.EnumerateFiles(directory, fileName, SearchOption.AllDirectories).FirstOrDefault();

                if (File.Exists(filePath))
                {
                    iniFile = new IniFile(filePath);
                    break;
                }
            }

            if ((object)iniFile == null)
                throw new InvalidOperationException(string.Format("Unable to find line configuration file for meter {0}", assetKey));

            m_assetKey = assetKey;

            m_lineSections = iniFile
                .GetSectionNames()
                .Where(sectionName => iniFile.GetSectionKeys(sectionName).Contains(LineIDKey))
                .Select(sectionName => new LineSection(schema, iniFile, sectionName))
                .ToArray();
        }

        public LineConfiguration(MeterDataSet meterDataSet, string[] directories)
            : this(meterDataSet, directories, new string[0])
        {
        }

        public LineConfiguration(MeterDataSet meterDataSet, string[] directories, string[] ignoreDirectories)
        {
            string assetKey = meterDataSet.Meter.AssetKey;
            string fileName = string.Format("{0}Lines.inf", assetKey);

            ignoreDirectories = ignoreDirectories
                .Where(path => !string.IsNullOrEmpty(path))
                .Select(path => new DirectoryInfo(path).FullName.TrimEnd(Path.DirectorySeparatorChar))
                .ToArray();

            Func<string, bool> isIgnored = filePath =>
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                return ignoreDirectories.Any(dir => dir.Equals(directoryPath, StringComparison.OrdinalIgnoreCase));
            };

            string iniFilePath = directories
                .SelectMany(directory => FilePath.EnumerateFiles(directory, fileName))
                .Where(filePath => !isIgnored(filePath))
                .FirstOrDefault();

            if ((object)iniFilePath == null)
                throw new InvalidOperationException(string.Format("Unable to find line configuration file for meter {0}", assetKey));

            IniFile iniFile = new IniFile(iniFilePath);

            m_assetKey = assetKey;

            m_lineSections = iniFile
                .GetSectionNames()
                .Where(sectionName => Regex.IsMatch(sectionName, "Line#[0-9]+$", RegexOptions.IgnoreCase))
                .Select(sectionName => new LineSection(meterDataSet, iniFile, sectionName))
                .ToArray();
        }

        #endregion

        #region [ Properties ]

        public string AssetKey
        {
            get
            {
                return m_assetKey;
            }
        }

        public LineSection[] LineSections
        {
            get
            {
                return m_lineSections;
            }
        }

        #endregion
    }
}
