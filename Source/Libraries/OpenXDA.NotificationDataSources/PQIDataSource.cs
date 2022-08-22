//******************************************************************************************************
//  PQIDataSource.cs - Gbtc
//
//  Copyright © 2022, Grid Protection Alliance.  All Rights Reserved.
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
//  01/13/2022 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using GSF.Configuration;
using GSF.Data;
using openXDA.Configuration;
using openXDA.Model;
using openXDA.PQI;

namespace openXDA.NotificationDataSources
{
    public class PQIDataSource : ITriggeredDataSource
    {
        #region [ Members ]

        // Nested Types
        private class DataSourceSettings
        {
            public DataSourceSettings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(PQISection.CategoryName)]
            public PQISection PQISettings { get; }
                      = new PQISection();
        }

        #endregion

        #region [ Constructors ]

        public PQIDataSource(Func<AdoDataConnection> xdaConnectionFactory) =>
            XDAConnectionFactory = xdaConnectionFactory;

        #endregion

        #region [ Properties ]
        private Func<AdoDataConnection> XDAConnectionFactory { get; }
        private DataSourceSettings Settings { get; set; }

        #endregion

        #region [ Methods ]

        public void Configure(Action<object> configurator) =>
            Settings = new DataSourceSettings(configurator);

        public XElement Process(Event evt)
        {
             string FetchAccessToken()
                {
                    NetworkCredential clientCredential = new NetworkCredential(Settings.PQISettings.ClientID, Settings.PQISettings.ClientSecret);
                    NetworkCredential userCredential = new NetworkCredential(Settings.PQISettings.Username, Settings.PQISettings.Password);
                    PingClient pingClient = new PingClient(Settings.PQISettings.PingURL);
                    Task exchangeTask = pingClient.ExchangeAsync(clientCredential, userCredential);
                    exchangeTask.GetAwaiter().GetResult();
                    return pingClient.AccessToken;
                }

            PQIWSClient pqiwsClient = new PQIWSClient(Settings.PQISettings.BaseURL, FetchAccessToken);
            PQIWSQueryHelper queryhelper = new PQIWSQueryHelper(XDAConnectionFactory, pqiwsClient);
            if (!queryhelper.HasImpactedComponentsAsync(evt.ID).Result)
                return new XElement("ImpactedEquipment");
            XElement result = new XElement("ImpactedEquipment");
            queryhelper.GetAllImpactedEquipmentAsync(evt.ID).Result.ForEach((equipment) =>
            {
                XElement node = new XElement("Equipment");
                node.SetAttributeValue("Facility", equipment.Facility);
                node.SetAttributeValue("SectionTitle", equipment.SectionTitle);
                node.SetAttributeValue("Area", equipment.Area);
                node.SetAttributeValue("SectionRank", equipment.SectionRank);
                node.SetAttributeValue("ComponentModel", equipment.ComponentModel);
                node.SetAttributeValue("Manufacturer", equipment.Manufacturer);
                node.SetAttributeValue("Series", equipment.Series);
                node.SetAttributeValue("ComponentType", equipment.ComponentType);

                result.Add(node);

            });

            return result;
        }

        #endregion
    }
}
