//******************************************************************************************************
//  ValueList.cs - Gbtc
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
//  09/10/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System;
using GSF.ComponentModel.DataAnnotations;
using GSF.Data;
using GSF.Data.Model;
using GSF.Web.Model;

namespace SystemCenter.Model
{
    [PostRoles("Administrator, Transmission SME")]
    [PatchRoles("Administrator, Transmission SME")]
    [GetRoles("Administrator, Transmission SME")]
    [AllowSearch]
    [ TableName("ValueList"), UseEscapedName, PrimaryLabel("Text"), SettingsCategory("systemSettings")]
    public class ValueList
    {

        [PrimaryKey(true)]
        public int ID { get; set; }

        [ParentKey(typeof(ValueListGroup))]
        public int GroupID { get; set; }
        public string Value { get; set; }
        public string AltValue { get; set; }
        public int SortOrder { get; set; }

    }

    public class ValueListController<T> : ModelController<T, ValueList>
        where T : ValueList, new()
    {
        [HttpGet, Route("Group/{groupName}")]
        public IHttpActionResult GetValueListForGroup(string groupName)
        {
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                TableOperations<ValueListGroup> groupTable = new TableOperations<ValueListGroup>(connection);
                TableOperations<ValueList> valueTable = new TableOperations<ValueList>(connection);
                List<int> groupIds = groupTable.QueryRecordsWhere("Name = {0}", groupName).Select(group => group.ID).ToList();
                if (groupIds.Count() == 0)
                {
                    RestrictedValueList restriction = RestrictedValueList.List.Find((g) => g.Name == groupName);
                    if (!(restriction is null))
                    {
                        groupTable.AddNewRecord(
                            new ValueListGroup()
                            {
                                Description = "",
                                Name = restriction.Name
                            });
                        groupIds.Add(connection.ExecuteScalar<int>("SELECT @@IDENTITY"));

                        int sortOrder = 1;
                        foreach (object item in restriction.DefaultItems)
                        {
                            string value;
                            string altValue;
                            if (item.GetType() == typeof(Tuple<string, string>))
                            {
                                value = ((Tuple<string, string>)item).Item1;
                                altValue = ((Tuple<string, string>)item).Item2;
                            }
                            else if (item.GetType() == typeof(string))
                            {
                                value = (string)item;
                                altValue = (string)item;
                            }
                            else
                                throw new InvalidCastException($"Could not convert object in DefaultItems of value list {restriction.Name} to either tuple or string.");
                            valueTable.AddNewRecord(
                                new ValueList()
                                {
                                    GroupID = groupIds[0],
                                    Value = value,
                                    AltValue = altValue,
                                    SortOrder = sortOrder
                                });
                            sortOrder++;
                        }
                    }
                    else
                        return Ok(new List<ValueList>());
                }
                IEnumerable<ValueList> records = valueTable.QueryRecordsWhere("GroupID in ({0})", string.Join(", ", groupIds)).OrderBy(v => v.SortOrder);
                return Ok(records);
            }
        }

        public override IHttpActionResult Patch([FromBody] ValueList newRecord)
        {
            if (!PatchAuthCheck())
            {
                return Unauthorized();
            }

            // Check if Value changed
            bool changeVal = false;
            ValueList oldRecord;

            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                oldRecord = new TableOperations<ValueList>(connection).QueryRecordWhere("ID = {0}", newRecord.ID);
                changeVal = !(newRecord.Value == oldRecord.Value);
            }

            if (changeVal)
            {
                ValueListGroup group;
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    group = new TableOperations<ValueListGroup>(connection).QueryRecordWhere("ID = {0}", newRecord.GroupID);
                    // Wrapping is needed here, since C# tries to use the wrong method signature otherwise
                    object[] parameters = new object[] { newRecord.Value, oldRecord.Value, group.Name };
                    // Update Additional Fields
                    connection.ExecuteScalar(@"UPDATE 
                        AdditionalFieldValue
                        SET [Value] = {0} 
                        WHERE
                        [Value] = {1} AND
                        (
                            SELECT TOP 1 Type
                            FROM AdditionalField 
                            WHERE AdditionalField.ID = AdditionalFieldValue.AdditionalFieldID
                        ) = {2}", parameters);

                    RestrictedValueList restriction = RestrictedValueList.List.Find((g) => g.Name == group.Name);
                    if (!(restriction?.UpdateSQL is null))
                    {
                        object[] updateSqlParams = new object[] { newRecord.Value, oldRecord.Value };
                        connection.ExecuteScalar(restriction.UpdateSQL, updateSqlParams);
                    }
                }
            }
            return base.Patch(newRecord);

        }

        public override IHttpActionResult Delete(ValueList record)
        {
            if (!DeleteAuthCheck())
            {
                return Unauthorized();
            }

            ValueListGroup group;
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                group = new TableOperations<ValueListGroup>(connection).QueryRecordWhere("ID = {0}", record.GroupID);
                RestrictedValueList restriction = RestrictedValueList.List.Find((g) => g.Name == group.Name);
                if (!(restriction?.CountSQL is null))
                {
                    int count = connection.ExecuteScalar<int>(restriction.CountSQL, record.Value);
                    if (count > 0)
                        return Unauthorized();
                }

                connection.ExecuteScalar(@"DELETE FROM AdditionalFieldValue
                            WHERE
                            [Value] = {0} AND
                            (SELECT TOP 1 Type FROM AdditionalField AF WHERE AF.ID = AdditionalFieldValue.AdditionalFieldID) = {1}", (object)record.Value, group.Name);

                return base.Delete(record);
            }

        }

        [Route("Count/{groupName}/{value}"), HttpGet]
        public IHttpActionResult GetCount(string groupName, string value)
        {
            if (!PatchAuthCheck())
                return Unauthorized();
            using (AdoDataConnection connection = new AdoDataConnection(Connection))
            {
                int nAddlFields = connection.ExecuteScalar<int>(@"SELECT COUNT(AFV.ID) FROM AdditionalFieldValue AFV WHERE 
                        [Value] = {0} AND (SELECT TOP 1 AF.ID FROM AdditionalField AF WHERE Type = {1}) = AFV.AdditionalFieldID
                        ", value, groupName);
                RestrictedValueList restriction = RestrictedValueList.List.Find((g) => g.Name == groupName);
                int count = 0;
                if (!(restriction?.CountSQL is null))
                {
                    count = connection.ExecuteScalar<int>(restriction.CountSQL, value);
                }
                return Ok(nAddlFields + count);
            }

        }
    }
}