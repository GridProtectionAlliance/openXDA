//******************************************************************************************************
//  FaultResultsWriterBase.cs - Gbtc
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
//  --------------------------------------------------------------------------------------------------- 
//  Portions of this work are derived from "openFLE" which is an Electric Power Research Institute, Inc.
//  (EPRI) copyrighted open source software product released under the BSD license.  openFLE carries
//  the following copyright notice: Version 1.0 - Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC.
//  All rights reserved.
//  ---------------------------------------------------------------------------------------------------
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  11/13/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using GSF.Adapters;
using GSF.Configuration;

namespace FaultAlgorithms
{
    /// <summary>
    /// Base class for writing the results of the fault analysis to an output source.
    /// </summary>
    public abstract class FaultResultsWriterBase : Adapter, IFaultResultsWriter
    {
        /// <summary>
        /// Writes configuration information to the output source.
        /// </summary>
        /// <param name="deviceConfigurations">Configuration information to be written to the output source.</param>
        public virtual void WriteConfiguration(ICollection<Device> deviceConfigurations)
        {
        }

        /// <summary>
        /// Writes the results to the output source.
        /// </summary>
        /// <param name="disturbanceRecorder">The device that collected the disturbance data.</param>
        /// <param name="disturbanceFiles">Information about the data files collected during the disturbance.</param>
        /// <param name="lineDataSets">The data sets used for analysis to determine fault location.</param>
        public virtual void WriteResults(Device disturbanceRecorder, ICollection<DisturbanceFile> disturbanceFiles, ICollection<Tuple<Line, FaultLocationDataSet>> lineDataSets)
        {
        }

        /// <summary>
        /// Loads saved <see cref="FaultResultsWriterBase"/> settings from the config file if the <see cref="P:GSF.Adapters.Adapter.PersistSettings"/> property is set to true.
        /// </summary>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException"><see cref="P:GSF.Adapters.Adapter.SettingsCategory"/> has a value of null or empty string.</exception>
        public override void LoadSettings()
        {
            base.LoadSettings();

            if (PersistSettings)
            {
                // Load settings from the specified category.
                ConfigurationFile config = ConfigurationFile.Current;
                CategorizedSettingsElementCollection settings = config.Settings[SettingsCategory];
                settings.Add("Enabled", "False", "Determines if fault results writer should be enabled at startup.");
                Enabled = settings["Enabled"].ValueAsBoolean(false);
            }
        }

        /// <summary>
        /// Saves <see cref="FaultResultsWriterBase"/> settings to the config file if the <see cref="P:GSF.Adapters.Adapter.PersistSettings"/> property is set to true.
        /// </summary>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException"><see cref="P:GSF.Adapters.Adapter.SettingsCategory"/> has a value of null or empty string.</exception>
        public override void SaveSettings()
        {
            base.SaveSettings();

            if (PersistSettings)
            {
                // Load settings from the specified category.
                ConfigurationFile config = ConfigurationFile.Current;
                CategorizedSettingsElementCollection settings = config.Settings[SettingsCategory];
                settings["Enabled", true].Update(Enabled);
                config.Save();
            }
        }
    }
}
