//*********************************************************************************************************************
// FaultLocationEngine.cs
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
//  05/16/2012 - J. Ritchie Carroll, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using FaultAlgorithms;
using GSF;
using GSF.Configuration;
using GSF.IO;

namespace openFLE
{
    /// <summary>
    /// Represents an engine that processes power quality data
    /// to determine the locations of faults along power lines.
    /// </summary>
    public class FaultLocationEngine : IDisposable
    {
        #region [ Members ]

        // Events

        /// <summary>
        /// Triggered when a message concerning the status
        /// of the fault location engine is encountered.
        /// </summary>
        public event EventHandler<EventArgs<string>> StatusMessage;

        /// <summary>
        /// Triggered when an exception is handled by the fault location engine.
        /// </summary>
        public event EventHandler<EventArgs<Exception>> ProcessException;

        // Fields
        private string m_deviceDefinitionsFile;
        private double m_processDelay;
        private string m_dropFolder;
        private string m_lengthUnits;
        private int m_debugLevel;
        private string m_debugFolder;

        private string[] m_fileExtensionFilter = { "*.pqd", "*.d00", "*.dat" };
        private Dictionary<string, DateTime> m_fileCreationTimes = new Dictionary<string, DateTime>();
        private System.Timers.Timer m_fileMonitor;

        private volatile ICollection<Device> m_devices;
        private IFaultResultsWriter m_faultResultsWriter;

        private Logger m_currentLogger;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Finalizer for <see cref="FaultLocationEngine"/> class.
        /// </summary>
        ~FaultLocationEngine()
        {
            Dispose();
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Starts the fault location engine.
        /// </summary>
        public void Start()
        {
            // Make sure default service settings exist
            ConfigurationFile configFile = ConfigurationFile.Current;

            // System settings
            // TODO: Add description to system settings
            CategorizedSettingsElementCollection systemSettings = configFile.Settings["systemSettings"];
            systemSettings.Add("DeviceDefinitionsFile", "DeviceDefinitions.xml", "");
            systemSettings.Add("ProcessDelay", "15", "");
            systemSettings.Add("DropFolder", "Drop", "");
            systemSettings.Add("LengthUnits", "Miles", "");
            systemSettings.Add("DebugLevel", "1", "");
            systemSettings.Add("DebugFolder", "Debug", "");

            // Retrieve file paths as defined in the config file
            m_deviceDefinitionsFile = FilePath.GetAbsolutePath(systemSettings["DeviceDefinitionsFile"].Value);
            m_processDelay = systemSettings["ProcessDelay"].ValueAs(m_processDelay);
            m_dropFolder = FilePath.AddPathSuffix(FilePath.GetAbsolutePath(systemSettings["DropFolder"].Value));
            m_lengthUnits = systemSettings["LengthUnits"].Value;
            m_debugLevel = systemSettings["DebugLevel"].ValueAs(m_debugLevel);
            m_debugFolder = FilePath.AddPathSuffix(FilePath.GetAbsolutePath(systemSettings["DebugFolder"].Value));

            // Load the fault results writer defined in systemSettings
            LoadFaultResultsWriter(systemSettings, out m_faultResultsWriter);

            try
            {
                // Make sure file path directories exist
                if (!Directory.Exists(m_dropFolder))
                    Directory.CreateDirectory(m_dropFolder);

                if (m_debugLevel > 0 && !Directory.Exists(m_debugFolder))
                    Directory.CreateDirectory(m_debugFolder);
            }
            catch (Exception ex)
            {
                OnProcessException(new InvalidOperationException(string.Format("Failed to create directory due to exception: {0}", ex.Message), ex));
            }

            // Setup new simple file monitor - we do this since the .NET 4.0 FileWatcher has a bad memory leak :-(
            if ((object)m_fileMonitor == null)
            {
                m_fileMonitor = new System.Timers.Timer();
                m_fileMonitor.Interval = 1000;
                m_fileMonitor.AutoReset = false;
                m_fileMonitor.Elapsed += FileMonitor_Elapsed;
            }

            // Start watching for files
            m_fileMonitor.Start();
        }

        /// <summary>
        /// Stops the fault location engine.
        /// </summary>
        public void Stop()
        {
            if ((object)m_fileMonitor != null)
                m_fileMonitor.Stop();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            // Stop file monitor timer
            if ((object)m_fileMonitor != null)
            {
                m_fileMonitor.Elapsed -= FileMonitor_Elapsed;
                m_fileMonitor.Dispose();
            }
            m_fileMonitor = null;
        }

        /// <summary>
        /// Updates the device definitions by reloading the device definitions file.
        /// </summary>
        public void ReloadDeviceDefinitionsFile()
        {
            // Parses the device definitions file to get
            // the collection of devices defined there
            m_devices = CreateDevices(m_deviceDefinitionsFile);
            m_faultResultsWriter.WriteConfiguration(m_devices);
        }

        private void LoadFaultResultsWriter(CategorizedSettingsElementCollection systemSettings, out IFaultResultsWriter faultResultsWriter)
        {
            string resultsAssemblyName = "FaultAlgorithms.dll";
            string resultsTypeName = "FaultAlgorithms.XmlFaultResultsWriter";
            string resultsParameters = "resultsDirectory=Results";

            // TODO: Add descriptions to config file settings
            systemSettings.Add("ResultsAssembly", resultsAssemblyName, "");
            systemSettings.Add("ResultsType", resultsTypeName, "");
            systemSettings.Add("ResultsParameters", resultsParameters, "");
            resultsAssemblyName = FilePath.GetAbsolutePath(systemSettings["ResultsAssembly"].Value);
            resultsTypeName = systemSettings["ResultsType"].Value;
            resultsParameters = systemSettings["ResultsParameters"].Value;

            faultResultsWriter = LoadType<IFaultResultsWriter>(resultsAssemblyName, resultsTypeName);
            faultResultsWriter.Parameters = resultsParameters.ParseKeyValuePairs();
        }

        private T LoadType<T>(string assemblyName, string typeName) where T : class
        {
            Assembly assembly;
            Type type;

            try
            {
                assembly = Assembly.LoadFrom(assemblyName);
                type = assembly.GetType(typeName);
                return Activator.CreateInstance(type) as T;
            }
            catch (Exception ex)
            {
                OnProcessException(new InvalidOperationException(string.Format("Failed while loading {0} due to exception: {1}", typeof(T).Name, ex.Message), ex));
            }

            return null;
        }

        private void FileMonitor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Queue files from the unprocessed file directory that match the desired file patterns
            foreach (string fileName in FilePath.GetFileList(m_dropFolder))
            {
                if (FilePath.IsFilePatternMatch(m_fileExtensionFilter, FilePath.GetFileName(fileName), true))
                {
                    if (CanProcessFile(fileName))
                    {
                        ProcessFile(fileName);
                        m_fileCreationTimes.Remove(fileName);
                    }
                }
            }

            if ((object)m_fileMonitor != null)
                m_fileMonitor.Start();
        }

        private bool CanProcessFile(string fileName)
        {
            string rootFileName;
            string extension;

            string cfgFileName;
            TimeSpan timeSinceCreation;

            if ((object)fileName == null || !File.Exists(fileName))
                return false;

            rootFileName = FilePath.GetFileNameWithoutExtension(fileName);
            extension = FilePath.GetExtension(fileName).ToLowerInvariant().Trim();

            cfgFileName = Path.Combine(m_dropFolder, rootFileName + ".cfg");
            timeSinceCreation = DateTime.Now - GetFileCreationTime(fileName);

            // If the data file is COMTRADE and the schema file does not exist, the file cannot be processed
            if (string.Compare(extension, ".pqd", true) != 0 && !File.Exists(cfgFileName))
                return false;

            // If the extension for the data file is .d00, inject a delay before allowing the file to be processed
            if (string.Compare(extension, ".d00", true) == 0 && timeSinceCreation.TotalSeconds < m_processDelay)
                return false;

            return FilePath.GetFileList(Path.Combine(m_dropFolder, rootFileName + ".*"))
                .Where(file => !file.EndsWith(".fwr", true, CultureInfo.CurrentCulture))
                .All(file => File.Exists(file + ".fwr"));
        }

        private void ProcessFile(string fileName)
        {
            DateTime timeStarted;
            DateTime timeProcessed;

            string rootFileName;
            ICollection<Device> devices;
            FaultLocationDataSet faultDataSet;

            Device disturbanceRecorder;
            ICollection<DisturbanceFile> disturbanceFiles;
            ICollection<Tuple<Line, FaultLocationDataSet>> lineDataSets;

            timeStarted = DateTime.UtcNow;
            m_currentLogger = null;
            rootFileName = null;

            try
            {
                rootFileName = FilePath.GetFileNameWithoutExtension(fileName);

                if (m_debugLevel >= 1)
                    m_currentLogger = Logger.Open(string.Format("{0}{1}.log", m_debugFolder, rootFileName));

                OnStatusMessage(string.Format("Processing {0}...", fileName));

                // Make sure device definitions exist, and attempt to load them if they don't
                devices = m_devices;

                if ((object)devices == null)
                {
                    devices = CreateDevices(m_deviceDefinitionsFile);
                    m_faultResultsWriter.WriteConfiguration(devices);
                    m_devices = devices;
                }

                // Get the definition for the device that captured this event
                disturbanceRecorder = GetDevice(devices, rootFileName);
                disturbanceFiles = CreateDisturbanceFiles(rootFileName);
                lineDataSets = new Collection<Tuple<Line, FaultLocationDataSet>>();

                foreach (Line line in disturbanceRecorder.Lines)
                {
                    try
                    {
                        // Provide status information about which line is being processed
                        OnStatusMessage("Detecting faults on line {0}...", line.Name);

                        // Get the fault data set for this line
                        faultDataSet = GetFaultDataSet(fileName, line);

                        // Get the maximum rated current from the line definition
                        faultDataSet.RatedCurrent = line.Rating50F;

                        // Export data to CSV for validation
                        if (m_debugLevel >= 1)
                        {
                            MeasurementDataSet.ExportToCSV(FilePath.GetAbsolutePath(string.Format("{0}{1}.{2}_measurementData.csv", m_debugFolder, rootFileName, line.ID)), faultDataSet.Voltages, faultDataSet.Currents);
                            CycleDataSet.ExportToCSV(FilePath.GetAbsolutePath(string.Format("{0}{1}.{2}_cycleData.csv", m_debugFolder, rootFileName, line.ID)), faultDataSet.Cycles);
                        }

                        // Run fault trigger, type, and location algorithms
                        if (ExecuteFaultTriggerAlgorithm(line.FaultAlgorithmsSet.FaultTriggerAlgorithm, faultDataSet, line.FaultAlgorithmsSet.FaultTriggerParameters))
                        {
                            faultDataSet.FaultType = ExecuteFaultTypeAlgorithm(line.FaultAlgorithmsSet.FaultTypeAlgorithm, faultDataSet, line.FaultAlgorithmsSet.FaultTypeParameters);
                            faultDataSet.FaultDistance = ExecuteFaultLocationAlgorithm(line.FaultAlgorithmsSet.FaultLocationAlgorithm, faultDataSet, line.FaultAlgorithmsSet.FaultLocationParameters);
                            OnStatusMessage("Distance to fault: {0} {1}", faultDataSet.FaultDistance, m_lengthUnits);

                            // Add the line-specific parameters to the faultResultsWriter
                            lineDataSets.Add(Tuple.Create(line, faultDataSet));
                        }
                        else
                        {
                            OnStatusMessage("No fault detected.");
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Error detecting faults on line {0}: {1}", line.Name, ex.Message);
                        OnProcessException(new Exception(message, ex));
                    }
                }

                // Write results to the output source
                if (lineDataSets.Count > 0)
                {
                    timeProcessed = DateTime.UtcNow;

                    foreach (DisturbanceFile disturbanceFile in disturbanceFiles)
                    {
                        disturbanceFile.FLETimeStarted = timeStarted;
                        disturbanceFile.FLETimeProcessed = timeProcessed;
                    }

                    m_faultResultsWriter.WriteResults(disturbanceRecorder, disturbanceFiles, lineDataSets);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Unable to process file \"{0}\" due to exception: {1}", fileName, ex.Message);
                OnProcessException(new InvalidOperationException(errorMessage, ex));
            }

            try
            {
                if ((object)m_currentLogger != null)
                {
                    m_currentLogger.Close();
                    m_currentLogger = null;
                }
            }
            catch (Exception ex)
            {
                OnProcessException(ex);
            }

            try
            {
                if ((object)rootFileName != null)
                {
                    // Get a list of processed files based on file prefix
                    List<string> processedFiles = FilePath.GetFileList(string.Format("{0}{1}.*", m_dropFolder, rootFileName))
                        .Concat(FilePath.GetFileList(string.Format("{0}{1}_*", m_dropFolder, rootFileName)))
                        .ToList();

                    // Delete processed files from the drop folder
                    foreach (string file in processedFiles)
                        File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                OnProcessException(ex);
            }

            OnStatusMessage("");
        }

        private ICollection<Device> CreateDevices(string deviceDefinitionsFile)
        {
            XDocument deviceDefinitionsDocument;
            XElement root;

            FaultAlgorithmsSet defaultFaultAlgorithms;
            ICollection<Device> devices;

            if (!File.Exists(deviceDefinitionsFile))
                throw new FileNotFoundException(string.Format("Device definitions file \"{0}\" not found.", deviceDefinitionsFile));

            deviceDefinitionsDocument = XDocument.Load(deviceDefinitionsFile);
            root = deviceDefinitionsDocument.Root ?? new XElement("openFLE");
            defaultFaultAlgorithms = GetFaultAlgorithms(root.Element("analytics") ?? new XElement("analytics"));
            devices = new Collection<Device>();

            foreach (XElement deviceDefinition in root.Elements("device"))
                devices.Add(CreateDevice(deviceDefinition, defaultFaultAlgorithms));

            return devices;
        }

        private Device CreateDevice(XElement deviceDefinition, FaultAlgorithmsSet defaultFaultAlgorithms)
        {
            XElement deviceAttributes;
            XElement deviceLines;
            Device device;

            // Get the attributes and lines XML elements
            deviceAttributes = deviceDefinition.Element("attributes") ?? new XElement("attributes");
            deviceLines = deviceDefinition.Element("lines") ?? new XElement("lines");

            // Create new device and set the attributes
            device = new Device()
            {
                ID = (string)deviceDefinition.Attribute("id"),
                Make = (string)deviceAttributes.Element("make"),
                Model = (string)deviceAttributes.Element("model"),
                StationID = (string)deviceAttributes.Element("stationID"),
                StationName = (string)deviceAttributes.Element("stationName")
            };

            // Add lines based on the line definitions
            foreach (XElement lineDefinition in deviceLines.Elements("line"))
                device.Lines.Add(CreateLine(lineDefinition, defaultFaultAlgorithms));

            // Return the device
            return device;
        }

        private Line CreateLine(XElement lineDefinition, FaultAlgorithmsSet defaultFaultAlgorithms)
        {
            Line line;
            XElement impedancesElement;
            FaultAlgorithmsSet faultAlgorithms;

            double rating50F;
            double length;

            double r1;
            double x1;
            double r0;
            double x0;

            // Create new line
            line = new Line()
            {
                ID = (string)lineDefinition.Attribute("id"),
                Name = (string)lineDefinition.Element("name"),
                Voltage = (string)lineDefinition.Element("voltage"),
                EndStationID = (string)lineDefinition.Element("endStationID"),
                EndStationName = (string)lineDefinition.Element("endStationName"),
                ChannelsElement = lineDefinition.Element("channels") ?? new XElement("channels")
            };

            // Get the XML element that contains impedances
            impedancesElement = lineDefinition.Element("impedances") ?? new XElement("impedances");

            // Get fault algorithms for this line
            faultAlgorithms = GetFaultAlgorithms(lineDefinition);

            if ((object)faultAlgorithms.FaultTriggerAlgorithm == null)
            {
                faultAlgorithms.FaultTriggerAlgorithm = defaultFaultAlgorithms.FaultTriggerAlgorithm;
                faultAlgorithms.FaultTriggerParameters = defaultFaultAlgorithms.FaultTriggerParameters;
            }

            if ((object)faultAlgorithms.FaultTypeAlgorithm == null)
            {
                faultAlgorithms.FaultTypeAlgorithm = defaultFaultAlgorithms.FaultTypeAlgorithm;
                faultAlgorithms.FaultTypeParameters = defaultFaultAlgorithms.FaultTypeParameters;
            }

            if ((object)faultAlgorithms.FaultLocationAlgorithm == null)
            {
                faultAlgorithms.FaultLocationAlgorithm = defaultFaultAlgorithms.FaultLocationAlgorithm;
                faultAlgorithms.FaultLocationParameters = defaultFaultAlgorithms.FaultLocationParameters;
            }

            line.FaultAlgorithmsSet = faultAlgorithms;

            // Set parameters that require parsing
            if (double.TryParse((string)lineDefinition.Element("rating50F"), out rating50F))
                line.Rating50F = rating50F;

            if (double.TryParse((string)lineDefinition.Element("length"), out length))
                line.Length = length;

            if (double.TryParse((string)impedancesElement.Element("R1"), out r1))
                line.R1 = r1;

            if (double.TryParse((string)impedancesElement.Element("X1"), out x1))
                line.X1 = x1;

            if (double.TryParse((string)impedancesElement.Element("R0"), out r0))
                line.R0 = r0;

            if (double.TryParse((string)impedancesElement.Element("X0"), out x0))
                line.X0 = x0;

            // Return the line
            return line;
        }

        private FaultAlgorithmsSet GetFaultAlgorithms(XElement parentElement)
        {
            FaultAlgorithmsSet faultAlgorithms = new FaultAlgorithmsSet();

            LoadFaultTriggerAlgorithm(parentElement, out faultAlgorithms.FaultTriggerAlgorithm, out faultAlgorithms.FaultTriggerParameters);
            LoadFaultTypeAlgorithm(parentElement, out faultAlgorithms.FaultTypeAlgorithm, out faultAlgorithms.FaultTypeParameters);
            LoadFaultLocationAlgorithm(parentElement, out faultAlgorithms.FaultLocationAlgorithm, out faultAlgorithms.FaultLocationParameters);

            return faultAlgorithms;
        }

        private void LoadFaultTriggerAlgorithm(XElement parentElement, out FaultTriggerAlgorithm faultTriggerAlgorithm, out string parameters)
        {
            LoadFaultAlgorithm(parentElement, "faultTrigger", out faultTriggerAlgorithm, out parameters);
        }

        private void LoadFaultTypeAlgorithm(XElement parentElement, out FaultTypeAlgorithm faultTypeAlgorithm, out string parameters)
        {
            LoadFaultAlgorithm(parentElement, "faultType", out faultTypeAlgorithm, out parameters);
        }

        private void LoadFaultLocationAlgorithm(XElement parentElement, out FaultLocationAlgorithm faultLocationAlgorithm, out string parameters)
        {
            LoadFaultAlgorithm(parentElement, "faultLocation", out faultLocationAlgorithm, out parameters);
        }

        private void LoadFaultAlgorithm<T>(XElement parentElement, string elementName, out T faultAlgorithm, out string parameters) where T : class
        {
            XElement faultElement = parentElement.Element(elementName) ?? new XElement(elementName);

            faultAlgorithm = LoadAlgorithm<T>(faultElement);
            parameters = LoadAlgorithmParameters(faultElement);
        }

        private T LoadAlgorithm<T>(XElement algorithmElement) where T : class
        {
            XAttribute assemblyName = algorithmElement.Attribute("assembly");
            XAttribute algorithmName = algorithmElement.Attribute("method");

            if ((object)assemblyName != null && (object)algorithmName != null)
                return LoadAlgorithm<T>(FilePath.GetAbsolutePath(assemblyName.Value), algorithmName.Value);

            return null;
        }

        private string LoadAlgorithmParameters(XElement algorithmElement)
        {
            return algorithmElement.Elements()
                .ToDictionary(element => element.Name.LocalName, element => element.Value)
                .JoinKeyValuePairs();
        }

        private T LoadAlgorithm<T>(string assemblyName, string algorithmName) where T : class
        {
            int index;
            string typeName;
            string methodName;

            Assembly assembly;
            Type type;
            MethodInfo method;

            try
            {
                index = algorithmName.LastIndexOf('.');
                typeName = algorithmName.Substring(0, index);
                methodName = algorithmName.Substring(index + 1);

                assembly = Assembly.LoadFrom(assemblyName);
                type = assembly.GetType(typeName);
                method = type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.InvokeMethod);

                return Delegate.CreateDelegate(typeof(T), method) as T;
            }
            catch (Exception ex)
            {
                OnProcessException(new InvalidOperationException(string.Format("Failed while loading {0} due to exception: {1}", typeof(T).Name, ex.Message), ex));
            }

            return null;
        }

        private ICollection<DisturbanceFile> CreateDisturbanceFiles(string rootFileName)
        {
            ICollection<DisturbanceFile> disturbanceFiles = new Collection<DisturbanceFile>();
            string fwrPattern = Path.Combine(m_dropFolder, string.Format("{0}*.fwr", rootFileName));
            string[] fwrList = FilePath.GetFileList(fwrPattern);

            foreach (string fwrPath in fwrList)
                disturbanceFiles.Add(CreateDisturbanceFile(fwrPath));

            return disturbanceFiles;
        }

        private DisturbanceFile CreateDisturbanceFile(string filePath)
        {
            XDocument fwrDocument = XDocument.Load(filePath);
            XElement root = fwrDocument.Root ?? new XElement("fileWatcherResults");
            XElement sourceFullPath = root.Element("sourceFullPath") ?? new XElement("sourceFullPath");
            XElement stats = root.Element("Stats") ?? new XElement("Stats");

            int fileSize;
            DateTime creationTime;
            DateTime lastWriteTime;
            DateTime lastAccessTime;
            DateTime startTime;
            TimeSpan processingTime;

            DisturbanceFile disturbanceFile;

            disturbanceFile = new DisturbanceFile()
            {
                SourcePath = (string)sourceFullPath,
                DestinationPath = (string)root.Element("DestinationFullPath")
            };

            if (int.TryParse((string)sourceFullPath.Attribute("Size"), out fileSize))
                disturbanceFile.FileSize = fileSize;

            if (DateTime.TryParse((string)sourceFullPath.Attribute("CreationTime"), out creationTime))
                disturbanceFile.CreationTime = creationTime;

            if (DateTime.TryParse((string)sourceFullPath.Attribute("LastWriteTime"), out lastWriteTime))
                disturbanceFile.LastWriteTime = lastWriteTime;

            if (DateTime.TryParse((string)sourceFullPath.Attribute("LastAccessTime"), out lastAccessTime))
                disturbanceFile.LastAccessTime = lastAccessTime;

            if (DateTime.TryParse((string)stats.Attribute("StartTime"), out startTime))
            {
                disturbanceFile.FileWatcherTimeStarted = startTime;

                if (TimeSpan.TryParse((string)stats.Attribute("ProcessingTime"), out processingTime))
                    disturbanceFile.FileWatcherTimeProcessed = startTime + processingTime;
            }

            return disturbanceFile;
        }

        private Device GetDevice(ICollection<Device> devices, string rootFileName)
        {
            return devices.FirstOrDefault(device => (object)device.ID != null && rootFileName.StartsWith(device.ID, StringComparison.CurrentCultureIgnoreCase));
        }

        private FaultLocationDataSet GetFaultDataSet(string fileName, Line line)
        {
            string extension = FilePath.GetExtension(fileName).ToLowerInvariant().Trim();

            // Load the fault data set based on provided parameters
            MeasurementDataSet voltageMeasurementDataSet = new MeasurementDataSet();
            MeasurementDataSet currentMeasurementDataSet = new MeasurementDataSet();
            CycleDataSet cycleDataSet;
            FaultLocationDataSet faultDataSet;

            StringBuilder parameters = new StringBuilder();

            // TODO: Load other needed specific associated parameters based on file / line information once all meta-data is known and defined
            parameters.AppendFormat("fileName={0}; ", fileName);
            parameters.Append("dataSourceType=");

            // Process files based on file extension
            switch (extension)
            {
                case ".pqd":
                    parameters.Append("PQDIF");
                    break;
                case ".d00":
                case ".dat":
                    parameters.Append("Comtrade");
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown file extension encountered: \"{0}\" - cannot parse file.", extension));
            }

            // Load data sets based on specified parameters
            LoadDataSets(voltageMeasurementDataSet, currentMeasurementDataSet, parameters.ToString(), line);
            cycleDataSet = new CycleDataSet(voltageMeasurementDataSet, currentMeasurementDataSet, GetSampleRate(voltageMeasurementDataSet.AN.Times));

            faultDataSet = new FaultLocationDataSet(voltageMeasurementDataSet, currentMeasurementDataSet, cycleDataSet)
            {
                PositiveImpedance = new ComplexNumber(line.R1, line.X1),
                ZeroImpedance = new ComplexNumber(line.R0, line.X0),
                LineDistance = line.Length
            };

            return faultDataSet;
        }

        // Attempts to execute fault trigger algorithm and processes errors if they occur.
        private bool ExecuteFaultTriggerAlgorithm(FaultTriggerAlgorithm algorithm, FaultLocationDataSet faultDataSet, string parameters)
        {
            try
            {
                return algorithm(faultDataSet, parameters);
            }
            catch (Exception ex)
            {
                OnProcessException(new Exception(string.Format("Error executing fault trigger algorithm: {0}", ex.Message), ex));
                return false;
            }
        }

        // Attempts to execute fault type algorithm and processes errors if they occur.
        private FaultType ExecuteFaultTypeAlgorithm(FaultTypeAlgorithm algorithm, FaultLocationDataSet faultDataSet, string parameters)
        {
            try
            {
                return algorithm(faultDataSet, parameters);
            }
            catch (Exception ex)
            {
                OnProcessException(new Exception(string.Format("Error executing fault type algorithm: {0}", ex.Message), ex));
                return FaultType.None;
            }
        }

        // Attempts to execute fault location algorithm and processes errors if they occur.
        private double ExecuteFaultLocationAlgorithm(FaultLocationAlgorithm algorithm, FaultLocationDataSet faultDataSet, string parameters)
        {
            try
            {
                return algorithm(faultDataSet, parameters);
            }
            catch (Exception ex)
            {
                OnProcessException(new Exception(string.Format("Error executing fault location algorithm: {0}", ex.Message), ex));
                return -1.0D;
            }
        }

        // Displays status message to the console - proxy method for service implementation
        private void OnStatusMessage(string format, params object[] args)
        {
            string message = string.Format(format, args);

            if ((object)m_currentLogger != null)
                m_currentLogger.WriteLine(message);

            if ((object)StatusMessage != null)
                StatusMessage(this, new EventArgs<string>(message));
        }

        // Displays exception message to the console - proxy method for service implmentation
        private void OnProcessException(Exception ex)
        {
            StringBuilder stackTrace;
            Exception inner;

            if ((object)m_currentLogger != null)
            {
                stackTrace = new StringBuilder();
                inner = ex;

                while ((object)inner != null)
                {
                    stackTrace.AppendLine(inner.StackTrace);
                    inner = inner.InnerException;
                }

                m_currentLogger.WriteLine(string.Empty);
                m_currentLogger.WriteLine(string.Format("ERROR: {0}", ex.Message));
                m_currentLogger.WriteLine(stackTrace.ToString());
                m_currentLogger.WriteLine(string.Empty);
            }

            if ((object)ProcessException != null)
                ProcessException(this, new EventArgs<Exception>(ex));
        }

        // Gets the creation time of the file with the given file name.
        private DateTime GetFileCreationTime(string fileName)
        {
            DateTime creationTime;

            if (!m_fileCreationTimes.TryGetValue(fileName, out creationTime))
            {
                creationTime = DateTime.Now;
                m_fileCreationTimes.Add(fileName, creationTime);
            }

            return creationTime;
        }

        #endregion

        #region [ Static ]

        // Static Methods

        // Load voltage and current data set based on connection string parameters
        private static void LoadDataSets(MeasurementDataSet voltageDataSet, MeasurementDataSet currentDataSet, string parameters, Line line)
        {
            if (string.IsNullOrEmpty(parameters))
                throw new ArgumentNullException("parameters");

            Dictionary<string, string> settings = parameters.ParseKeyValuePairs();
            string dataSourceType;

            if (!settings.TryGetValue("dataSourceType", out dataSourceType) || string.IsNullOrWhiteSpace(dataSourceType))
                throw new ArgumentException("Parameters must define a \"dataSourceType\" setting.");

            switch (dataSourceType.ToLowerInvariant().Trim())
            {
                case "pqdif":
                    PQDIFLoader.PopulateDataSets(settings, voltageDataSet, currentDataSet);
                    break;
                case "comtrade":
                    ComtradeLoader.PopulateDataSets(settings, voltageDataSet, currentDataSet, line);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("parameters", string.Format("Cannot parse \"{0}\" data source type - format is undefined.", dataSourceType));
            }

            // TODO: Determine if this is a valid assumption
            if ((object)voltageDataSet.AN.Values == null || voltageDataSet.AN.Values.Length == 0 || (object)voltageDataSet.BN.Values == null || voltageDataSet.BN.Values.Length == 0 || (object)voltageDataSet.CN.Values == null || voltageDataSet.CN.Values.Length == 0)
                throw new InvalidOperationException("Cannot calculate fault location without line-to-neutral values.");
        }

        private static int GetSampleRate(long[] times)
        {
            int[] knownSampleRates = { 96, 100, 128, 256 };

            long startTime;
            long cycleTime;
            int samples;
            int min;

            // If there are no times in the array, default to 128
            if ((object)times == null || times.Length <= 0)
                return 128;

            // Assume 60 Hz and get a full cycle
            startTime = times[0];
            cycleTime = Ticks.PerSecond / 60L;
            samples = times.TakeWhile(time => time - startTime < cycleTime).Count();

            // Return the known sample rate closest to the 60 Hz sample rate
            min = knownSampleRates.Min(sampleRate => Math.Abs(sampleRate - samples));
            return knownSampleRates.Single(sampleRate => Math.Abs(sampleRate - samples) == min);
        }

        #endregion
    }
}
