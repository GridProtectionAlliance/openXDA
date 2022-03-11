//******************************************************************************************************
//  SQLDataSource.cs - Gbtc
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
using System.Xml.Linq;
using GSF.Configuration;
using GSF.Data;
using openXDA.Model;

namespace openXDA.NotificationDataSources
{
    public class SQLDataSource : ITriggeredDataSource
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [SettingName("SQLStatement")]
            public string SQL { get; } = "SELECT NULL FOR XML PATH('Data')";
        }

        #endregion

        #region [ Constructors ]

        public SQLDataSource(TriggeredEmailDataSource definition, Func<AdoDataConnection> connectionFactory)
        {
            Definition = definition;
            Connectionfactory = connectionFactory;
        }

        #endregion

        #region [ Properties ]
      
        public TriggeredEmailDataSource Definition { get; }

        public Func<AdoDataConnection> Connectionfactory { get; }

        #endregion

        #region [ Methods ]
        
        protected Action<object> GetConfigurator()
        {
            int dataSoruceID = Definition.ID;

            ConfigurationLoader configurationLoader = new ConfigurationLoader(Definition.ID, Connectionfactory);
            return configurationLoader.Configure;
        }

        public XElement Process(Event evt)
        {
            try
            {
                Settings settings = new Settings(GetConfigurator());

                using (AdoDataConnection connection = Connectionfactory())
                {
                    return XElement.Parse(connection.ExecuteScalar<string>(settings.SQL, evt.ID));
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
