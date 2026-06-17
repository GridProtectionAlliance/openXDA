//******************************************************************************************************
//  WidgetConfigurationLoader.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  04/25/2025 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json.Linq;
using openXDA.Model;
using SEBrowser.Model;

namespace openXDA.Controllers.Widgets
{
    /// <summary>
    /// Loads settings from openXDA and the Widget table.
    /// </summary>
    /// <remarks>
    /// This is a derivative of the openXDA Configuration Loader but specifically for Widgets.
    /// </remarks>
    internal class WidgetConfigurationLoader
    {
        public WidgetConfigurationLoader(Func<AdoDataConnection> connectionFactory, int widgetID)
        {
            ConnectionFactory = connectionFactory;
            LazyConfigureAction = new Lazy<Action<object>>(() => CreateConfigureAction(widgetID));
        }

        private Func<AdoDataConnection> ConnectionFactory { get; }
        private Lazy<Action<object>> LazyConfigureAction { get; }

        public void Configure(object obj)
        {
            LazyConfigureAction.Value(obj);
        }

        private Action<object> CreateConfigureAction(int widgetID)
        {
            string connectionString = LoadConnectionString(widgetID);
            return obj => ConnectionStringParser.ParseConnectionString(connectionString, obj);
        }

        private string LoadConnectionString(int widgetID)
        {
            string ToConnectionString(IEnumerable<string[]> settingList, int index)
            {
                string ToValue(IEnumerable<string[]> grouping)
                {
                    return grouping.Any(setting => index < setting.Length - 2)
                        ? ToConnectionString(grouping, index + 1)
                        : grouping.First().Last();
                }

                return settingList
                    .Where(setting => index < setting.Length - 1)
                    .GroupBy(setting => setting[index], StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(grouping => grouping.Key, ToValue, StringComparer.OrdinalIgnoreCase)
                    .JoinKeyValuePairs();
            }

            using (AdoDataConnection connection = ConnectionFactory())
            {
                IEnumerable<string[]> xdaSettings = LoadXDASettings(connection);
                IEnumerable<string[]> widgetSettings = LoadWidgetSettings(connection, widgetID);
                return ToConnectionString(xdaSettings.Concat(widgetSettings), 0);
            }
        }

        private IEnumerable<string[]> LoadXDASettings(AdoDataConnection connection)
        {
            TableOperations<Setting> settingTable = new TableOperations<Setting>(connection);
            return settingTable.QueryRecords().Select(setting => ToArray(setting.Name, setting.Value)).ToList();
        }

        private IEnumerable<string[]> LoadWidgetSettings(AdoDataConnection connection, int widgetID)
        {
            string widgetSettings = new TableOperations<Widget>(connection).QueryRecord(new RecordRestriction("ID = {0}", widgetID)).Setting;
            return JsonConfigurationParser.Parse(widgetSettings).Select(setting => ToArray(setting.Key, setting.Value));
        }

        private string[] ToArray(string key, string value)
        {
            List<string> parts = key.Split('.').ToList();
            parts.Add(value);
            return parts.ToArray();
        }

        private static ConnectionStringParser<SettingAttribute, CategoryAttribute> ConnectionStringParser { get; } =
            new ConnectionStringParser<SettingAttribute, CategoryAttribute>();
    }

    internal class JsonConfigurationParser
    {
        private readonly IDictionary<string, string> m_data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> m_context = new Stack<string>();
        private string m_currentPath;

        public static IDictionary<string, string> Parse(string json)
        {
            return new JsonConfigurationParser().ParseJson(json);
        }

        private IDictionary<string, string> ParseJson(string json)
        {
            m_data.Clear();
            VisitJObject(JObject.Parse(json));
            return m_data;
        }

        private void VisitJObject(JObject jObject)
        {
            foreach (JProperty property in jObject.Properties())
            {
                EnterContext(property.Name);
                VisitToken(property.Value);
                ExitContext();
            }
        }

        private void VisitToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    VisitJObject(token.Value<JObject>());
                    break;
                case JTokenType.Array:
                    VisitArray(token.Value<JArray>());
                    break;
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Bytes:
                case JTokenType.Raw:
                case JTokenType.Null:
                    VisitPrimitive(token.Value<JValue>());
                    break;
                default:
                    throw new FormatException("Unsupported JSON token");
            }
        }

        private void VisitArray(JArray array)
        {
            for (int index = 0; index < array.Count; index++)
            {
                EnterContext(index.ToString());
                VisitToken(array[index]);
                ExitContext();
            }
        }

        private void VisitPrimitive(JValue data)
        {
            m_data[m_currentPath] = data.ToString(CultureInfo.InvariantCulture);
        }

        private void EnterContext(string context)
        {
            m_context.Push(context);
            m_currentPath = string.Join(":", m_context.Reverse());
        }

        private void ExitContext()
        {
            m_context.Pop();
            m_currentPath = string.Join(":", m_context.Reverse());
        }
    }
}


