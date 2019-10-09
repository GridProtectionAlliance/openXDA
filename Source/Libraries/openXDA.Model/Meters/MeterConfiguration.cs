//******************************************************************************************************
//  MeterConfiguration.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  09/27/2019 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSF.Data.Model;
using GSF.Text;

namespace openXDA.Model
{
    public class MeterConfiguration
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int MeterID { get; set; }

        public int? DiffID { get; set; }

        [StringLength(50)]
        public string ConfigKey { get; set; }

        public string ConfigText { get; set; }
    }

    public static partial class TableOperationsExtensions
    {
        public static string Unpatch(this TableOperations<MeterConfiguration> meterConfigurationTable, MeterConfiguration meterConfiguration)
        {
            DiffMatchPatch patchProvider = new DiffMatchPatch();

            string ToSheetText(MeterConfiguration config)
            {
                if (config.DiffID == null)
                    return config.ConfigText;

                MeterConfiguration configToPatch = meterConfigurationTable.QueryRecordWhere("ID = {0}", config.DiffID);
                List<Patch> patches = patchProvider.PatchFromText(config.ConfigText);
                string sheetToPatch = ToSheetText(configToPatch);
                return (string)patchProvider.PatchApply(patches, sheetToPatch)[0];
            }

            return ToSheetText(meterConfiguration);
        }

        public static void Patch(this TableOperations<MeterConfiguration> meterConfigurationTable, MeterConfiguration meterConfiguration, string newConfigText)
        {
            string unpatchedText = Unpatch(meterConfigurationTable, meterConfiguration);

            DiffMatchPatch patchProvider = new DiffMatchPatch();
            List<Patch> patches = patchProvider.PatchMake(newConfigText, unpatchedText);

            if (patches.Count > 0)
            {
                MeterConfiguration newConfiguration = new MeterConfiguration();
                newConfiguration.MeterID = meterConfiguration.MeterID;
                newConfiguration.ConfigKey = meterConfiguration.ConfigKey;
                newConfiguration.ConfigText = newConfigText;
                meterConfigurationTable.AddNewRecord(newConfiguration);

                meterConfiguration.DiffID = meterConfigurationTable.Connection.ExecuteScalar<int>("SELECT @@IDENTITY");
                meterConfiguration.ConfigText = patchProvider.PatchToText(patches);
                meterConfigurationTable.UpdateRecord(meterConfiguration);
            }
        }

        public static void PatchLatestConfiguration(this TableOperations<MeterConfiguration> meterConfigurationTable, Meter meter, string configKey, string newConfigText)
        {
            RecordRestriction latestConfigurationQueryRestriction =
                new RecordRestriction("MeterID = {0}", meter.ID) &
                new RecordRestriction("ConfigKey = {0}", configKey) &
                new RecordRestriction("DiffID IS NULL");

            MeterConfiguration latestConfiguration = meterConfigurationTable.QueryRecord("ID DESC", latestConfigurationQueryRestriction);

            if (latestConfiguration == null)
            {
                MeterConfiguration newConfiguration = new MeterConfiguration();
                newConfiguration.MeterID = meter.ID;
                newConfiguration.ConfigKey = configKey;
                newConfiguration.ConfigText = newConfigText;
                meterConfigurationTable.AddNewRecord(newConfiguration);
                return;
            }

            meterConfigurationTable.Patch(latestConfiguration, newConfigText);
        }
    }
}
