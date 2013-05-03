//*********************************************************************************************************************
// ComtradeLoader.cs
// Version 1.1 and subsequent releases
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
// --------------------------------------------------------------------------------------------------------------------
//
// Version 1.0
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openFLE ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
// •    Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
// •    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
// •    Neither the name of the Electric Power Research Institute, Inc. (“EPRI”) nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL EPRI BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
//
//
// This software incorporates work covered by the following copyright and permission notice: 
//
// •    TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
//      No copyright is claimed pursuant to 17 USC § 105. All Other Rights Reserved.
//
//      Licensed under TVA Custom License based on NASA Open Source Agreement (TVA Custom NOSA); 
//      you may not use TVA Code Library except in compliance with the TVA Custom NOSA. You may  
//      obtain a copy of the TVA Custom NOSA at http://tvacodelibrary.codeplex.com/license.
//
//      TVA Code Library is provided by the copyright holders and contributors "as is" and any express 
//      or implied warranties, including, but not limited to, the implied warranties of merchantability 
//      and fitness for a particular purpose are disclaimed.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  05/23/2012 - J. Ritchie Carroll, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Comtrade;
using FaultAlgorithms;
using GSF.IO;

namespace openFLE
{
    /// <summary>
    /// Defines functions needed to load COMTRADE data sets for openFLE.
    /// </summary>
    internal static class ComtradeLoader
    {
        /// <summary>
        /// Populate known voltage and current data from PQDIF file.
        /// </summary>
        /// <param name="settings">Source parameters.</param>
        /// <param name="voltageDataSet">Voltage data set to populate.</param>
        /// <param name="currentDataSet">Current data set to populate.</param>
        /// <param name="line">Associated XML event file definition.</param>
        public static void PopulateDataSets(Dictionary<string, string> settings, MeasurementDataSet voltageDataSet, MeasurementDataSet currentDataSet, Line line)
        {
            string fileName;

            if ((object)line == null)
                throw new ArgumentNullException("line");

            if (!settings.TryGetValue("fileName", out fileName) || !File.Exists(fileName))
                throw new ArgumentException("Parameters must define a valid \"fileName\" setting.");

            // Comtrade parsing will require a CFG file, make sure this exists...
            string directory = Path.GetDirectoryName(fileName) ?? string.Empty;
            string rootFileName = FilePath.GetFileNameWithoutExtension(fileName);
            string configurationFileName = Path.Combine(directory, rootFileName + ".cfg");

            if (!File.Exists(configurationFileName))
                throw new FileNotFoundException(string.Format("Associated CFG file \"{0}\" for COMTRADE data file does not exist - cannot parse COMTRADE file.", configurationFileName));

            // Parse configuration file
            Schema schema = new Schema(configurationFileName);

            // Find <Channels> element in XML line definition
            XElement channels = line.ChannelsElement;

            if ((object)channels == null)
                throw new NullReferenceException("No \"<channels>\" element was found in event file definition - cannot load COMTRADE data file.");

            // Extract COMTRADE channel ID's for desired voltage and current elements
            IEnumerable<Tuple<int, int>> vaIndexes = GetValueIndex(schema, channels, "VA").ToList();
            IEnumerable<Tuple<int, int>> vbIndexes = GetValueIndex(schema, channels, "VB").ToList();
            IEnumerable<Tuple<int, int>> vcIndexes = GetValueIndex(schema, channels, "VC").ToList();
            IEnumerable<Tuple<int, int>> iaIndexes = GetValueIndex(schema, channels, "IA").ToList();
            IEnumerable<Tuple<int, int>> ibIndexes = GetValueIndex(schema, channels, "IB").ToList();
            IEnumerable<Tuple<int, int>> icIndexes = GetValueIndex(schema, channels, "IC").ToList();

            List<long> times = new List<long>();
            List<double> vaValues = new List<double>();
            List<double> vbValues = new List<double>();
            List<double> vcValues = new List<double>();
            List<double> iaValues = new List<double>();
            List<double> ibValues = new List<double>();
            List<double> icValues = new List<double>();

            ValidateIndexes("VA", vaIndexes);
            ValidateIndexes("VB", vbIndexes);
            ValidateIndexes("VC", vcIndexes);
            ValidateIndexes("IA", iaIndexes);
            ValidateIndexes("IB", ibIndexes);
            ValidateIndexes("IC", icIndexes);

            // Create a new COMTRADE file parser
            using (Parser parser = new Parser()
            {
                Schema = schema,
                FileName = fileName,
                InferTimeFromSampleRates = true
            })
            {
                // Open COMTRADE data files
                parser.OpenFiles();

                // Read all COMTRADE records
                while (parser.ReadNext())
                {
                    times.Add(parser.Timestamp.Ticks);
                    vaValues.Add(GetValue(parser, vaIndexes));
                    vbValues.Add(GetValue(parser, vbIndexes));
                    vcValues.Add(GetValue(parser, vcIndexes));
                    iaValues.Add(GetValue(parser, iaIndexes));
                    ibValues.Add(GetValue(parser, ibIndexes));
                    icValues.Add(GetValue(parser, icIndexes));
                }
            }

            // Populate voltage data set
            voltageDataSet.AN.Times = times.ToArray();
            voltageDataSet.AN.Values = vaValues.ToArray();
            voltageDataSet.BN.Times = times.ToArray();
            voltageDataSet.BN.Values = vbValues.ToArray();
            voltageDataSet.CN.Times = times.ToArray();
            voltageDataSet.CN.Values = vcValues.ToArray();

            // Populate current data set
            currentDataSet.AN.Times = times.ToArray();
            currentDataSet.AN.Values = iaValues.ToArray();
            currentDataSet.BN.Times = times.ToArray();
            currentDataSet.BN.Values = ibValues.ToArray();
            currentDataSet.CN.Times = times.ToArray();
            currentDataSet.CN.Values = icValues.ToArray();

        }

        // Gets the actual analog value indexes based on the schema based channel indexes
        private static IEnumerable<Tuple<int, int>> GetValueIndex(Schema schema, XElement channels, string channelName)
        {
            XElement element = channels.Element(channelName);
            string[] channelValues;
            int channelIndex, multiplier;
            string units;

            if ((object)element == null)
                throw new NullReferenceException(string.Format("No \"{0}\" element was found in defined \"<Channels>\" of device definition - cannot load COMTRADE data file.", channelName));

            channelValues = element.Value.Split(',');

            // Index defined in COMTRADE schema may start at any number, return actual index into value array
            foreach (string channelValue in channelValues)
            {
                if (!int.TryParse(channelValue, out channelIndex))
                    throw new InvalidOperationException(string.Format("The \"{0}\" element in the device definition was not a comma-separated list of integers - cannot load COMTRADE data file.", channelName));

                channelIndex = Math.Abs(channelIndex);
                multiplier = (channelValue.Trim()[0] == '-') ? -1 : 1;

                for (int valueIndex = 0; valueIndex < schema.AnalogChannels.Length; valueIndex++)
                {
                    if (schema.AnalogChannels[valueIndex].Index == channelIndex)
                    {
                        units = schema.AnalogChannels[valueIndex].Units;

                        if (units.Contains("kA") || units.Contains("kV"))
                            multiplier *= 1000;

                        yield return new Tuple<int, int>(multiplier, valueIndex);
                        break;
                    }
                }
            }
        }

        // Gets the actual value as the combination of the given indexes.
        private static double GetValue(Parser parser, IEnumerable<Tuple<int, int>> indexes)
        {
            return indexes.Sum(index => index.Item1 * parser.Values[index.Item2]);
        }

        // Validates indexes to determine if the specified channels exist in the COMTRADE file.
        private static void ValidateIndexes(string channel, IEnumerable<Tuple<int, int>> indexes)
        {
            if ((object)indexes == null || !indexes.Any())
                throw new InvalidOperationException(string.Format("Unable to find channel indexes for channel {0}. Channel mappings may be incorrect.", channel));
        }
    }
}