//******************************************************************************************************
//  AzureDataSource.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
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
//  04/27/2023 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using GSF.Data;
using GSF.Data.Model;
using GSF.Security;
using log4net;
using Microsoft.Graph;
using openXDA.Model;
using SystemCenter.Model;
using User = Microsoft.Graph.User;

namespace openXDA.NotificationDataSources
{
    public class AzureDataSource : IScheduledDataSource
    {
        #region [ Members ]

        // Nested Types
        private class DataSourceSettings
        {
            public DataSourceSettings(Action<object> configure) =>
                configure(this);

            [Setting]
            [DefaultValue("JobTitle")]
            public string AzureFieldName { get; set; }

            [Setting]
            [DefaultValue("Position")]
            public string XDAFieldName { get; set; }
        }

        private class Update
        {
            public ConfirmableUserAccount User { get; set; }
            public AdditionalUserFieldValue Updated { get; set; } = null;
            public AdditionalUserFieldValue Previous { get; set; } = null;
        }

        // Fields
        private AzureADSettings m_azureADSettings;
        private GraphServiceClient m_graphClient;

        #endregion

        #region [ Constructors ]

        public AzureDataSource(Func<AdoDataConnection> connectionFactory) =>
            ConnectionFactory = connectionFactory;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets Azure AD settings.
        /// </summary>
        public AzureADSettings AzureADSettings
        {
            get
            {
                if (m_azureADSettings == null)
                    m_azureADSettings = AzureADSettings.Load();
                return m_azureADSettings;
            }
        }

        /// <summary>
        /// Gets Graph client.
        /// </summary>
        public GraphServiceClient GraphClient
        {
            get
            {
                if (m_graphClient is null)
                    m_graphClient = AzureADSettings.GetGraphClient();
                return m_graphClient;
            }
        }

        private Func<AdoDataConnection> ConnectionFactory { get; }
        private DataSourceSettings Settings { get; set; }

        #endregion

        #region [ Methods ]

        public void Configure(Action<object> configurator) =>
            Settings = new DataSourceSettings(configurator);

        public XElement Process(DateTime xdaNow)
        {
            XElement result = new XElement("ADFieldUpdate");
            result.SetAttributeValue("AzureFieldName", Settings.AzureFieldName);

            // get all Users
            using (AdoDataConnection connection = ConnectionFactory())
            {
                TableOperations<ConfirmableUserAccount> userTable = new TableOperations<openXDA.Model.ConfirmableUserAccount>(connection);
                IEnumerable<ConfirmableUserAccount> users = userTable.QueryRecords().Where(u => IsValidAzureADUserName(u.Name).Result);

                if (users.Count() == 0)
                    return result;

                TableOperations<AdditionalUserField> addlFieldTbl = new TableOperations<AdditionalUserField>(connection);
                AdditionalUserField addlFld = addlFieldTbl.QueryRecordWhere("FieldName = {0}", Settings.XDAFieldName);
                if (addlFld is null)
                {
                    addlFld = new AdditionalUserField()
                    {
                        FieldName = Settings.XDAFieldName,
                        Type = "string",
                        IsSecure = false,
                    };
                    addlFieldTbl.AddNewRecord(addlFld);
                    addlFld = addlFieldTbl.QueryRecordWhere("FieldName = {0}", Settings.XDAFieldName);
                    Log.Info($"Added Additional UserField {Settings.XDAFieldName} to Database");
                }

                IEnumerable<Update> updates =
                    users.Select(u => GetUpdate(u, connection, addlFld))
                    .Where(item => !(item.Updated is null) && (item.Previous is null || item.Previous.Value != (item.Updated.Value)));

                TableOperations<AdditionalUserFieldValue> addlFieldValTbl = new TableOperations<AdditionalUserFieldValue>(connection);

                result.Add(updates.Select(item =>
                {
                    XElement xml = new XElement("Update");
                    xml.SetAttributeValue("UserName", item.User.AccountName);
                    xml.SetAttributeValue("Value", item.Updated.Value);
                    xml.SetAttributeValue("PreviousValue", item.Previous?.Value ?? "N/A");
                    addlFieldValTbl.AddNewOrUpdateRecord(item.Updated);

                    return xml;
                }));

                return result;
            }
        }

        /// <summary>
        /// Gets flag that determines if specified user name can be found on Azure AD.
        /// </summary>
        /// <param name="userName">User name to lookup</param>
        /// <returns><c>true</c> if User name was found in Azure AD; otherwise, <c>false</c>.</returns>
        private async Task<bool> IsValidAzureADUserName(string userName)
        {
            GraphServiceClient graphClient = GraphClient;

            if (graphClient is null)
                return false;

            try
            {
                User user = await LoadAzureUserAsync(userName);
                return !(user is null);
            }
            catch (ServiceException ex)
            {
                if (ex.Error.Code == "Request_ResourceNotFound")
                    return false;
                else
                    throw new Exception("Unable to query Azure", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception attempting to query Azure", ex);
            }
        }

        private Update GetUpdate(ConfirmableUserAccount user, AdoDataConnection connection, AdditionalUserField field)
        {
            Update update = new Update() { User = user };
            TableOperations<AdditionalUserFieldValue> addlFieldTbl = new TableOperations<AdditionalUserFieldValue>(connection);
            AdditionalUserFieldValue addlFld = addlFieldTbl.QueryRecordWhere("AdditionalUserFieldID = {0} AND UserAccountID = {1}", field.ID, user.ID);
            update.Previous = addlFld;

            User adUser = LoadAzureUserAsync(user.Name)
                .GetAwaiter()
                .GetResult();

            if (addlFld is null)
            {
                update.Updated = new AdditionalUserFieldValue()
                {
                    AdditionalUserFieldID = field.ID,
                    UserAccountID = user.ID,
                    Value = ""
                };
            }
            else
            {
                update.Updated = new AdditionalUserFieldValue()
                {
                    ID = addlFld.ID,
                    AdditionalUserFieldID = addlFld.AdditionalUserFieldID,
                    UserAccountID = addlFld.UserAccountID,
                    Value = addlFld.Value
                };
            }

            string adValue = "";
            if (!(user.GetType().GetProperty(Settings.AzureFieldName) is null))
                adValue = user.GetType().GetProperty(Settings.AzureFieldName).GetValue(user, null).ToString();
            else if (adUser.AdditionalData.ContainsKey(Settings.AzureFieldName))
                adValue = adUser.AdditionalData[Settings.AzureFieldName].ToString();

            if (string.IsNullOrEmpty(adValue))
                update.Updated = null;
            else
                update.Updated.Value = adValue;

            return update;
        }

        private async Task<User> LoadAzureUserAsync(string username)
        {
            GraphServiceClient graphClient = GraphClient;
            string escapedUsername = Uri.EscapeUriString(username.Replace("'", "''"));

            if (!username.Contains("#EXT#"))
            {
                return await graphClient.Users[escapedUsername]
                    .Request()
                    .GetAsync();
            }

            // External users need to be looked up by userPrincipalName
            IGraphServiceUsersCollectionPage page = await graphClient.Users
                .Request()
                .Filter($"userPrincipalName eq '{escapedUsername}'")
                .GetAsync();

            return page.FirstOrDefault();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(AzureDataSource));

        #endregion
    }
}
