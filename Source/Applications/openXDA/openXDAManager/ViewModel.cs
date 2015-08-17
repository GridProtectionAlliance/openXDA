//*********************************************************************************************************************
// ViewModel.cs
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
// openXDA ("this software") is licensed under BSD 3-Clause license.
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
//  07/18/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using FaultAlgorithms;
using GSF;

namespace openXDAManager
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region [ Members ]

        // Constants
        private const string ConfigFile = "openXDA.exe.config";

        // Nested Types
        public class ResultsWriter
        {
            public string Assembly;
            public string TypeName;

            public ResultsWriter()
            {
            }

            public ResultsWriter(string assembly, string typeName)
            {
                Assembly = assembly;
                TypeName = typeName;
            }
        }

        // Events

        public event PropertyChangedEventHandler PropertyChanged;

        // Fields

        private XDocument m_dataModel;
        private List<ResultsWriter> m_resultsWriters;

        private string m_deviceDefinitionsFile;
        private string m_dropFolder;
        private string m_processDelay;
        private string m_lengthUnits;
        private string m_resultsAssembly;
        private string m_resultsType;
        private string m_resultsParameters;
        private string m_debugLevel;
        private string m_debugFolder;

        private ObservableCollection<string> m_resultsWriterNames;
        private int m_selectedResultsWriter;
        private bool m_ignoreNotify;

        #endregion

        #region [ Constructors ]

        public ViewModel()
        {
            PropertyChanged += (sender, args) => HandlePropertyChanged(args.PropertyName);
        }

        #endregion

        #region [ Properties ]

        public string DeviceDefinitionsFile
        {
            get
            {
                return m_deviceDefinitionsFile;
            }
            set
            {
                m_deviceDefinitionsFile = value;
                OnPropertyChanged("DeviceDefinitionsFile");
            }
        }

        public string DropFolder
        {
            get
            {
                return m_dropFolder;
            }
            set
            {
                m_dropFolder = value;
                OnPropertyChanged("DropFolder");
            }
        }

        public string ProcessDelay
        {
            get
            {
                return m_processDelay;
            }
            set
            {
                double processDelay;

                m_processDelay = value;
                OnPropertyChanged("ProcessDelay");

                if (!double.TryParse(value, out processDelay))
                    throw new InvalidOperationException("Process delay must be a number.");
            }
        }

        public string LengthUnits
        {
            get
            {
                return m_lengthUnits;
            }
            set
            {
                m_lengthUnits = value;
                OnPropertyChanged("LengthUnits");
            }
        }

        public string ResultsAssembly
        {
            get
            {
                return m_resultsAssembly;
            }
            set
            {
                m_resultsAssembly = value;
                OnPropertyChanged("ResultsAssembly");
            }
        }

        public string ResultsType
        {
            get
            {
                return m_resultsType;
            }
            set
            {
                m_resultsType = value;
                OnPropertyChanged("ResultsType");
            }
        }

        public string ResultsParameters
        {
            get
            {
                return m_resultsParameters;
            }
            set
            {
                m_resultsParameters = value;
                OnPropertyChanged("ResultsParameters");
            }
        }

        public string DebugLevel
        {
            get
            {
                return m_debugLevel;
            }
            set
            {
                m_debugLevel = value;
                OnPropertyChanged("DebugLevel");
            }
        }

        public string DebugFolder
        {
            get
            {
                return m_debugFolder;
            }
            set
            {
                m_debugFolder = value;
                OnPropertyChanged("DebugFolder");
            }
        }

        public ObservableCollection<string> ResultsWriterNames
        {
            get
            {
                return m_resultsWriterNames;
            }
            set
            {
                m_resultsWriterNames = value;
                OnPropertyChanged("ResultsWriterNames");
            }
        }

        public int SelectedResultsWriter
        {
            get
            {
                return m_selectedResultsWriter;
            }
            set
            {
                m_selectedResultsWriter = value;
                OnPropertyChanged("SelectedResultsWriter");
            }
        }

        #endregion

        #region [ Methods ]

        public void Load()
        {
            // Set up data model
            m_dataModel = XDocument.Load(ConfigFile);
            m_resultsWriters = GetResultsWriters();

            // Set up view model
            DeviceDefinitionsFile = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "DeviceDefinitionsFile").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            DropFolder = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "DropFolder").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            ProcessDelay = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "ProcessDelay").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            LengthUnits = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "LengthUnits").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            ResultsAssembly = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "ResultsAssembly").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            ResultsType = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "ResultsType").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            ResultsParameters = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "ResultsParameters").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            DebugLevel = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "DebugLevel").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            DebugFolder = m_dataModel.Descendants("add").Where(setting => (string)setting.Attribute("name") == "DebugFolder").Select(setting => (string)setting.Attribute("value")).FirstOrDefault();
            ResultsWriterNames = new ObservableCollection<string>(m_resultsWriters.Select(resultsWriter => resultsWriter.TypeName));
            SelectedResultsWriter = -1;
        }

        public void Save()
        {
            bool saved = false;
            bool restartService = false;
            ServiceController controller = null;

            XElement systemSettings;
            XElement deviceDefinitionsFileSetting;
            XElement dropFolderSetting;
            XElement processDelaySetting;
            XElement lengthUnitsSetting;
            XElement resultsAssemblySetting;
            XElement resultsTypeSetting;
            XElement resultsParametersSetting;
            XElement debugLevelSetting;
            XElement debugFolderSetting;

            try
            {
                controller = new ServiceController("openXDA");

                if (controller.CanStop && controller.Status != ServiceControllerStatus.Stopped && controller.Status != ServiceControllerStatus.StopPending)
                {
                    controller.Stop();
                    controller.WaitForStatus(ServiceControllerStatus.Stopped);
                    restartService = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to stop service due to exception: {0} Service will not be restarted.", ex.Message.EnsureEnd('.')));
            }

            try
            {
                systemSettings =
                    (m_dataModel.Root ?? m_dataModel.GetOrAddElement("configuration"))
                    .GetOrAddElement("categorizedSettings")
                    .GetOrAddElement("systemSettings");

                deviceDefinitionsFileSetting = systemSettings.GetOrAddSetting("DeviceDefinitionsFile");
                dropFolderSetting = systemSettings.GetOrAddSetting("DropFolder");
                processDelaySetting = systemSettings.GetOrAddSetting("ProcessDelay");
                lengthUnitsSetting = systemSettings.GetOrAddSetting("LengthUnits");
                resultsAssemblySetting = systemSettings.GetOrAddSetting("ResultsAssembly");
                resultsTypeSetting = systemSettings.GetOrAddSetting("ResultsType");
                resultsParametersSetting = systemSettings.GetOrAddSetting("ResultsParameters");
                debugLevelSetting = systemSettings.GetOrAddSetting("DebugLevel");
                debugFolderSetting = systemSettings.GetOrAddSetting("DebugFolder");

                deviceDefinitionsFileSetting.AddOrUpdateAttribute("value", DeviceDefinitionsFile);
                dropFolderSetting.AddOrUpdateAttribute("value", DropFolder);
                processDelaySetting.AddOrUpdateAttribute("value", ProcessDelay);
                lengthUnitsSetting.AddOrUpdateAttribute("value", LengthUnits);
                resultsAssemblySetting.AddOrUpdateAttribute("value", ResultsAssembly);
                resultsTypeSetting.AddOrUpdateAttribute("value", ResultsType);
                resultsParametersSetting.AddOrUpdateAttribute("value", ResultsParameters);
                debugLevelSetting.AddOrUpdateAttribute("value", DebugLevel);
                debugFolderSetting.AddOrUpdateAttribute("value", DebugFolder);

                m_dataModel.Save(ConfigFile);
                saved = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Configuration changes could not be saved due to exception: {0}", ex.Message));
            }

            try
            {
                if (restartService && controller.Status == ServiceControllerStatus.Stopped)
                {
                    controller.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to restart service due to exception: {0}", ex.Message));
            }

            if (saved)
            {
                MessageBox.Show("Configuration changes saved successfully.");
            }
        }

        public void LaunchConsoleMonitor()
        {
            const string processName = "openXDAConsole";
            Process consoleProcess;

            if (Process.GetProcessesByName(processName).Length <= 0)
            {
                consoleProcess = Process.Start(string.Format("{0}.exe", processName));

                if ((object)consoleProcess != null)
                    consoleProcess.Close();
            }
        }

        private List<ResultsWriter> GetResultsWriters()
        {
            return typeof(IFaultResultsWriter)
                .LoadImplementations(true)
                .Select(type => new ResultsWriter(type.Assembly.Location, type.FullName))
                .ToList();
        }

        private void HandlePropertyChanged(string propertyName)
        {
            if (!m_ignoreNotify)
            {
                m_ignoreNotify = true;
                HandlePropertyChangedOnce(propertyName);
                m_ignoreNotify = false;
            }
        }

        private void HandlePropertyChangedOnce(string propertyName)
        {
            ResultsWriter resultsWriter;

            switch (propertyName)
            {
                case "SelectedResultsWriter":
                    if (m_selectedResultsWriter >= 0)
                    {
                        resultsWriter = m_resultsWriters[m_selectedResultsWriter];
                        ResultsAssembly = resultsWriter.Assembly;
                        ResultsType = resultsWriter.TypeName;

                        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => SelectedResultsWriter = -1));
                    }
                    break;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if ((object)PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    internal static class XmlExtensions
    {
        public static XElement GetOrAddElement(this XContainer container, string elementName)
        {
            XElement element = container.Element(elementName);

            if ((object)element == null)
            {
                element = new XElement(elementName);
                container.Add(element);
            }

            return element;
        }

        public static XElement GetOrAddSetting(this XContainer container, string settingName)
        {
            XElement setting = container.Elements().FirstOrDefault(element => settingName == (string)element.Attribute("name"));

            if ((object)setting == null)
            {
                setting = new XElement("add",
                    new XAttribute("name", settingName),
                    new XAttribute("value", string.Empty),
                    new XAttribute("description", string.Empty),
                    new XAttribute("encrypted", "False")
                );

                container.Add(setting);
            }

            return setting;
        }

        public static void AddOrUpdateAttribute(this XElement element, string name, string value)
        {
            XAttribute attribute = element.Attribute(name);

            if ((object)attribute != null)
            {
                attribute.Value = value;
            }
            else
            {
                attribute = new XAttribute(name, value);
                element.Add(attribute);
            }
        }
    }
}
