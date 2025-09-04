//******************************************************************************************************
//  SSAMSNode.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  02/08/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.Http;
using System.Web.Http.Controllers;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Parsing;
using log4net;
using openXDA.Configuration;
using openXDA.Model;

namespace openXDA.Nodes.Types.SSAMS
{
    public class SSAMSNode : NodeBase
    {
        #region [ Members ]

        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(SSAMSSection.CategoryName)]
            public SSAMSSection SSAMSSettings { get; } = new SSAMSSection();
        }

        private class SSAMSWebController : ApiController
        {
            private SSAMSNode Node { get; }

            public SSAMSWebController(SSAMSNode node) =>
                Node = node;

            [HttpGet]
            public void Reconfigure() =>
                Node.Reconfigure();
        }

        #endregion

        private const string TaskName = "DatabaseOperation";

        public SSAMSNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            Action<object> configurator = GetConfigurator();
            ScheduleDBSignal(configurator);
        }

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new SSAMSWebController(this);

        protected override void OnReconfigure(Action<object> configurator)
        {
            ScheduleDBSignal(configurator);
        }

        private void ScheduleDBSignal(Action<object> configurator)
        {
            Settings settings = new Settings(configurator);
            string schedule = settings.SSAMSSettings.Schedule;
            // We don't need to dispose this, its handled by host
            Host.RegisterScheduledProcess(this, DatabaseOperation, TaskName, schedule);
        }

        private void DatabaseOperation()
        {
            Log.Debug("Began scheduled SSAMS task.");
            Action<object> configurator = GetConfigurator();
            Settings settings = new Settings(configurator);
            if (settings.SSAMSSettings.DatabaseCommand is null)
            {
                Log.Warn("SSAMS task had no command specified, aborting...");
                return;
            }
            using (AdoDataConnection connection = new AdoDataConnection(settings.SSAMSSettings.ConnectionString, settings.SSAMSSettings.DataProviderString))
            {
                List<object> parameters = new List<object>();
                if (!(settings.SSAMSSettings.CommandParameters is null))
                {
                    Dictionary<string, string> substitutions = new Dictionary<string, string>
                    {
                        //["{Acronym}"] = Name,
                        ["{Timestamp}"] = DateTime.UtcNow.ToString(TimeTagBase.DefaultFormat)
                    };
                    TemplatedExpressionParser parameterTemplate = new TemplatedExpressionParser
                    {
                        TemplatedExpression = settings.SSAMSSettings.CommandParameters
                    };
                    parameters = new List<object>();
                    string[] commandParameters = parameterTemplate.Execute(substitutions).Split(',');

                    // Do some basic typing on command parameters
                    foreach (string commandParameter in commandParameters)
                    {
                        string parameter = commandParameter.Trim();

                        if (parameter.StartsWith("'") && parameter.EndsWith("'"))
                            parameters.Add(parameter.Length > 2 ? parameter.Substring(1, parameter.Length - 2) : "");
                        else if (int.TryParse(parameter, out int ival))
                            parameters.Add(ival);
                        else if (double.TryParse(parameter, out double dval))
                            parameters.Add(dval);
                        else if (bool.TryParse(parameter, out bool bval))
                            parameters.Add(bval);
                        else if (DateTime.TryParseExact(parameter, TimeTagBase.DefaultFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dtval))
                            parameters.Add(dtval);
                        else if (DateTime.TryParse(parameter, out dtval))
                            parameters.Add(dtval);
                        else
                            parameters.Add(parameter);
                    }
                }
                connection.ExecuteScalar(settings.SSAMSSettings.DatabaseCommand, parameters.ToArray());
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(SSAMSNode));

        #endregion
    }
}
