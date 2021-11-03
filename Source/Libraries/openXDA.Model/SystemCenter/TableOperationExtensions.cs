//******************************************************************************************************
//  TableOperationExtensions.cs - Gbtc
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
//  07/07/2021 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;
using GSF.Security.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCenter.Model
{
    public static class TableOperationExtensions
    {

        #region [ AdditionalUserField ]
        /// <summary>
        /// Gets Field if exists, if not, creates field and returns newly created field
        /// </summary>
        /// <param name="tableOperations"></param>
        /// <param name="fieldName">Name of field</param>
        /// <param name="fieldType">Name of field type (integer, string, etc) or name of value list category, defaults to string</param>
        /// <param name="isSecure">Labels field as secure or not, defaults to false</param>
        /// <returns></returns>
        public static AdditionalUserField GetField(this TableOperations<AdditionalUserField> tableOperations, string fieldName, string fieldType = "string", bool isSecure = false)
        {
            AdditionalUserField record = tableOperations.QueryRecordWhere("FieldName = {0}", fieldName);
            if (record != null) return record;

            record = new AdditionalUserField();
            record.FieldName = fieldName;
            record.Type = fieldType;
            record.IsSecure = isSecure;

            tableOperations.AddNewRecord(record);

            record = tableOperations.QueryRecordWhere("FieldName = {0}", fieldName);

            return record;
        }

        /// <summary>
        /// Gets AdditonalUserFieldValue record by user and field,if it exists, else returns new one with userid and fieldid prepopulated
        /// </summary>
        /// <param name="tableOperations"></param>
        /// <param name="userName">Name of user</param>
        /// <param name="fieldName">Name of field</param>
        /// <returns></returns>
        public static AdditionalUserFieldValue GetValue(this TableOperations<AdditionalUserFieldValue> tableOperations, string userName, string fieldName)
        {
            AdditionalUserField field = new TableOperations<AdditionalUserField>(tableOperations.Connection).GetField(fieldName);
            UserAccount userAccount = new TableOperations<UserAccount>(tableOperations.Connection).QueryRecordWhere("Name = {0}", userName);

            if (userAccount == null)
                return new AdditionalUserFieldValue() { ID = 0, UserAccountID = Guid.Empty, AdditionalUserFieldID = field.ID, Value = ""};

            AdditionalUserFieldValue value = tableOperations.QueryRecordWhere("UserAccountID = {0} AND AdditionalUserFieldID = {1}", userAccount.ID, field.ID);

            if (value == null)
                return new AdditionalUserFieldValue() { ID = 0, UserAccountID = userAccount.ID, AdditionalUserFieldID = field.ID, Value = "" };
            else
                return value;
        }

        #endregion

        #region [ AdditionalField ]

        /// <summary>
        /// Gets Field if exists, if not, creates field and returns newly created field
        /// </summary>
        /// <param name="tableOperations"></param>
        /// <param name="parentTable">Name of Parent Table</param>
        /// <param name="fieldName">Name of field</param>
        /// <param name="fieldType">Name of field type (integer, string, etc) or name of value list category, defaults to string</param>
        /// <param name="externalDB">Name of external database to retrieve this data, defaults to null</param>
        /// <param name="externalDBTable">Name of external database table to retrieve this data, defaults to null</param>
        /// <param name="externalDBTableKey">Key from external database table used to reference this data, defaults to null</param>
        /// <param name="isSecure">Labels field as secure or not, defaults to false</param>
        /// <returns></returns>
        public static AdditionalField GetField(this TableOperations<AdditionalField> tableOperations, string parentTable, string fieldName, string fieldType = "string", string externalDB = null, string externalDBTable = null, string externalDBTableKey = null ,bool isSecure = false)
        {
            AdditionalField record = tableOperations.QueryRecordWhere("ParentTable = {0} AND FieldName = {1}", parentTable, fieldName);
            if (record != null) return record;

            record = new AdditionalField();
            record.ParentTable = parentTable;
            record.FieldName = fieldName;
            record.Type = fieldType;
            record.ExternalDB = externalDB;
            record.ExternalDBTable = externalDBTable;
            record.ExternalDBTableKey = externalDBTableKey;
            record.IsSecure = isSecure;

            tableOperations.AddNewRecord(record);

            record = tableOperations.QueryRecordWhere("ParentTable = {0} AND FieldName = {1}", parentTable, fieldName);

            return record;
        }

        /// <summary>
        /// Gets AdditionalFieldValue record by user and field,if it exists
        /// </summary>
        /// <param name="tableOperations"></param>
        /// <param name="parentTable">Name of Parent Table</param>
        /// <param name="fieldName">Name of field</param>
        /// <returns></returns>
        public static AdditionalFieldValue GetValue(this TableOperations<AdditionalFieldValue> tableOperations, string parentTable, int parentTableID,string fieldName)
        {
            AdditionalField field = new TableOperations<AdditionalField>(tableOperations.Connection).GetField(parentTable, fieldName);
            AdditionalFieldValue value = tableOperations.QueryRecordWhere("ParentTableID = {0} AND AdditionalFieldID = {1}", parentTableID, field.ID);

            if (value == null)
                return new AdditionalFieldValue() { ID = 0, ParentTableID = parentTableID, AdditionalFieldID = field.ID, Value = "" };
            else
                return value;

        }

        #endregion

        #region [ ValueList ] 

        /// <summary>
        /// Gets Group if exists, if not, creates group and returns newly created group
        /// </summary>
        /// <param name="tableOperations"></param>
        /// <param name="name">Name of group</param>
        /// <param name="description">Description of group, defaults to empty string</param>
        /// <returns></returns>
        public static ValueListGroup GetGroup(this TableOperations<ValueListGroup> tableOperations, string name, string description = "")
        {
            ValueListGroup record = tableOperations.QueryRecordWhere("Name = {0}", name);
            if (record != null) return record;

            record = new ValueListGroup();
            record.Name = name;
            record.Description = description;

            tableOperations.AddNewRecord(record);

            record = tableOperations.QueryRecordWhere("Name = {0}", name);

            return record;
        }

        /// <summary>
        /// Gets ValueList record by group and value,if it exists
        /// </summary>
        /// <param name="tableOperations"></param>
        /// <param name="groupName">Name of group</param>
        /// <param name="value">Value</param>
        /// <param name="stringCompare">If true uses LIKE '%{value}%' in sql instead of using equation</param>
        /// <returns></returns>
        public static ValueList GetValue(this TableOperations<ValueList> tableOperations, string groupName, string value, bool stringCompare = false)
        {
            ValueListGroup group = new TableOperations<ValueListGroup>(tableOperations.Connection).GetGroup(groupName);

            if (stringCompare)
                return tableOperations.QueryRecordWhere("GroupID = {0} AND Value LIKE {1}", group.ID, "%"+value+"%");
            else
                return tableOperations.QueryRecordWhere("GroupID = {0} AND Value = {1}", group.ID, value);
        }

        /// <summary>
        /// Gets ValueList record by group and AltValue,if it exists, if not, creates new record and returns newly created record
        /// </summary>
        /// <param name="tableOperations"></param>
        /// <param name="groupName">Name of group</param>
        /// <param name="altValue">Alternate value for valuelist</param>
        /// <param name="stringCompare">If true uses LIKE '%{value}%' in sql instead of using equation</param>
        /// <returns></returns>
        public static ValueList GetAltValue(this TableOperations<ValueList> tableOperations, string groupName, string altValue, bool stringCompare = false)
        {
            ValueListGroup group = new TableOperations<ValueListGroup>(tableOperations.Connection).GetGroup(groupName);

            if (stringCompare)
                return tableOperations.QueryRecordWhere("GroupID = {0} AND AltValue LIKE {1}", group.ID, "%" + altValue + "%");
            else
                return tableOperations.QueryRecordWhere("GroupID = {0} AND AltValue = {1}", group.ID, altValue);
        }


        #endregion
    }

}

