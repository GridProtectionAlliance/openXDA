﻿//******************************************************************************************************
//  AppModel.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
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
//  01/21/2016 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Text;
using GSF.Data;
using GSF.Data.Model;
using GSF.Identity;
using GSF.Web;
using GSF.Web.Model;

namespace openXDA.Model
{
    /// <summary>
    /// Defines a base application model with convenient global settings and functions.
    /// </summary>
    /// <remarks>
    /// Custom view models should inherit from AppModel because the "Global" property is used by Layout.cshtml.
    /// </remarks>
    public class AppModel : IDisposable
    {
        #region [ Members ]

        private DataContext m_dataContext;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets global settings for application.
        /// </summary>
        public GlobalSettings Global { get; } = new GlobalSettings();

        // Gets reference to MiPlan context, creating it if needed
        public DataContext DataContext => m_dataContext ?? (m_dataContext = new DataContext("systemSettings"));

        #endregion

        #region [ Methods ]

        public void Dispose() =>
            m_dataContext?.Dispose();

        public bool DetectIE(string userAgent) =>
            userAgent.Contains("MSIE ") ||
            userAgent.Contains("Trident/") ||
            userAgent.Contains("Edge/");

        #endregion

        #region [ Static ]

        // Static Methods

        /// <summary>
        /// Renders client-side Javascript function for looking up single values from a table.
        /// </summary>
        /// <param name="valueFieldName">Table field name as defined in the table.</param>
        /// <param name="idFieldName">Name of primary key field, defaults to "ID".</param>
        /// <param name="lookupFunctionName">Name of lookup function, defaults to lookup + <paramref name="groupName"/>.ToTitleCase() + Value.</param>
        /// <param name="arrayName">Name of lookup function, defaults to lookup + <paramref name="groupName"/>.ToTitleCase() + Value.</param>
        /// <returns>Client-side Javascript lookup function.</returns>
        public static string RenderAbstract<T>(string valueFieldName, string idFieldName = "ID", string lookupFunctionName = null, string arrayName = null) where T : class, new()
        {
            StringBuilder javascript = new StringBuilder();
            DataContext dataContext = new DataContext();

            if (string.IsNullOrWhiteSpace(lookupFunctionName))
                lookupFunctionName = $"lookup{valueFieldName}Value";

            if (string.IsNullOrWhiteSpace(arrayName))
                arrayName = $"{valueFieldName}";
            TableOperations<T> operations = dataContext.Table<T>() as TableOperations<T>;

            javascript.AppendLine($"var {arrayName} = [];\r\n");
            foreach (T record in operations.QueryRecords())
            {
                var valueField = operations.GetFieldValue(record, valueFieldName);
                var idField = operations.GetFieldValue(record, idFieldName);

                javascript.AppendLine($"        {arrayName}[\"{idField.ToString().JavaScriptEncode()}\"] = \"{valueField?.ToString().JavaScriptEncode()}\";");
            }

            javascript.AppendLine($"\r\n        function {lookupFunctionName}(value) {{");
            javascript.AppendLine($"            return {arrayName}[value];");
            javascript.AppendLine("        }");

            return javascript.ToString();
        }

        public static bool ValidateAdminRequestForRole(string role, string userName)
        {
            string userid = UserInfo.UserNameToSID(userName);

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                bool isAdmin = connection.ExecuteScalar<int>(@"
					select 
						COUNT(*) 
					from 
						UserAccount JOIN 
						ApplicationRoleUserAccount ON ApplicationRoleUserAccount.UserAccountID = UserAccount.ID JOIN
						ApplicationRole ON ApplicationRoleUserAccount.ApplicationRoleID = ApplicationRole.ID
					WHERE 
						ApplicationRole.Name = 'Developer' AND UserAccount.Name = {0}
                ", userid) > 0;

                return isAdmin;
            }
        }

        #endregion
    }
}
