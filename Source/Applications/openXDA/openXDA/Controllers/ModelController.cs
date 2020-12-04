//******************************************************************************************************
//  ModelBaseController.cs - Gbtc
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
//  10/04/2019 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using GSF.Collections;
using GSF.Data;
using GSF.Data.Model;
using GSF.Identity;
using GSF.Reflection;
using GSF.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace openXDA.Controllers
{
    public class ModelController<T> : ApiController where T : class, new()
    {
        #region [Members ]

        public class Search
        {
            public string FieldName { get; set; }
            public string SearchText { get; set; }
            public string Operator { get; set; }
            public string Type { get; set; }

        }

        public class PostData
        {
            public IEnumerable<Search> Searches { get; set; }
            public string OrderBy { get; set; }
            public bool Ascending { get; set; }
        }

        #endregion

        #region [ Constructor ]
        public ModelController()
        {
        }

        public ModelController(bool hasParent, string parentKey, string primaryKeyField = "ID")
        {
            HasParent = hasParent;
            ParentKey = parentKey;
            HasUniqueKey = false;
            UniqueKeyField = "";
            PrimaryKeyField = "ID";

        }

        public ModelController(bool hasParent, string parentKey, bool hasUniqueKey, string uniqueKey)
        {
            HasParent = hasParent;
            ParentKey = parentKey;
            HasUniqueKey = hasUniqueKey;
            UniqueKeyField = uniqueKey;
        }

        #endregion

        #region [ Properties ]
        protected virtual bool HasParent { get; } = false;
        protected virtual string ParentKey { get; } = "";
        protected virtual string PrimaryKeyField { get; } = "ID";
        protected virtual bool HasUniqueKey { get; } = false;
        protected virtual string UniqueKeyField { get; } = "";
        protected virtual string Connection { get; } = "systemSettings";
        protected virtual string GetRoles { get; } = "Viewer,Administrator";
        protected virtual string PostRoles { get; } = "Administrator";
        protected virtual string PatchRoles { get; } = "Administrator";
        protected virtual string DeleteRoles { get; } = "Administrator";
        protected virtual string GetOrderByExpression { get; } = null;
        protected virtual bool ViewOnly { get; } = false;
        protected virtual bool AllowSearch { get; } = false;
        #endregion

        #region [ Http Methods ]
        [HttpGet, Route("New")]
        public virtual IHttpActionResult GetNew()
        {
            if (ViewOnly)
                return Unauthorized();

            if (GetRoles == string.Empty || User.IsInRole(GetRoles))
            {
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {

                    try
                    {
                        T record = new TableOperations<T>(connection).NewRecord();
                        return Ok(JsonConvert.SerializeObject(record));
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpGet, Route("{parentID?}")]
        public virtual IHttpActionResult Get(string parentID = null)
        {
            if (GetRoles == string.Empty || User.IsInRole(GetRoles))
            {
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {

                    try
                    {
                        IEnumerable<T> result;
                        if (HasParent && parentID != null)
                        {
                            PropertyInfo parentKey = typeof(T).GetProperty(ParentKey);
                            if (parentKey.PropertyType == typeof(int))
                                result = new TableOperations<T>(connection).QueryRecords(GetOrderByExpression, new RecordRestriction(ParentKey + " = {0}", int.Parse(parentID)));
                            else if (parentKey.PropertyType == typeof(Guid))
                                result = new TableOperations<T>(connection).QueryRecords(GetOrderByExpression, new RecordRestriction(ParentKey + " = {0}", Guid.Parse(parentID)));
                            else
                                result = new TableOperations<T>(connection).QueryRecords(GetOrderByExpression, new RecordRestriction(ParentKey + " = {0}", parentID));
                        }
                        else
                            result = new TableOperations<T>(connection).QueryRecords(GetOrderByExpression);

                        return Ok(JsonConvert.SerializeObject(result));
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }
            }
            else
            {
                return Unauthorized();
            }

        }
        [HttpGet, Route("{sort}/{ascending:int}")]
        public virtual IHttpActionResult Get(string sort, int ascending)
        {
            if (GetRoles == string.Empty || User.IsInRole(GetRoles))
            {
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    string orderByExpression = GetOrderByExpression;

                    if (sort != null && sort != string.Empty)
                        orderByExpression = $"{sort} {(ascending == 1 ? "ASC" : "DESC")}";

                    try
                    {
                        IEnumerable<T> result = new TableOperations<T>(connection).QueryRecords(orderByExpression);

                        return Ok(JsonConvert.SerializeObject(result));
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }
            }
            else
            {
                return Unauthorized();
            }

        }


        [HttpGet, Route("{parentID}/{sort}/{ascending:int}")]
        public virtual IHttpActionResult Get(string parentID, string sort, int ascending)
        {
            if (GetRoles == string.Empty || User.IsInRole(GetRoles))
            {
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    string orderByExpression = GetOrderByExpression;

                    if (sort != null && sort != string.Empty)
                        orderByExpression = $"{sort} {(ascending == 1 ? "ASC" : "DESC")}";

                    try
                    {
                        IEnumerable<T> result;
                        if (HasParent && parentID != null)
                        {
                            PropertyInfo parentKey = typeof(T).GetProperty(ParentKey);
                            if (parentKey.PropertyType == typeof(int))
                                result = new TableOperations<T>(connection).QueryRecords(orderByExpression, new RecordRestriction(ParentKey + " = {0}", int.Parse(parentID)));
                            else if (parentKey.PropertyType == typeof(Guid))
                                result = new TableOperations<T>(connection).QueryRecords(orderByExpression, new RecordRestriction(ParentKey + " = {0}", Guid.Parse(parentID)));
                            else
                                result = new TableOperations<T>(connection).QueryRecords(orderByExpression, new RecordRestriction(ParentKey + " = {0}", parentID));
                        }
                        else
                            result = new TableOperations<T>(connection).QueryRecords(orderByExpression);

                        return Ok(JsonConvert.SerializeObject(result));
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpGet, Route("One/{id}")]
        public virtual IHttpActionResult GetOne(string id)
        {
            if (GetRoles == string.Empty || User.IsInRole(GetRoles))
            {
                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    try
                    {
                        T result = null;
                        PropertyInfo primaryKey = typeof(T).GetProperty(PrimaryKeyField);
                        if (primaryKey.PropertyType == typeof(int))
                            result = new TableOperations<T>(connection).QueryRecordWhere(PrimaryKeyField + " = {0}", int.Parse(id));
                        else if (primaryKey.PropertyType == typeof(Guid))
                            result = new TableOperations<T>(connection).QueryRecordWhere(PrimaryKeyField + " = {0}", Guid.Parse(id));
                        else
                            result = new TableOperations<T>(connection).QueryRecordWhere(PrimaryKeyField + " = {0}", id);

                        if (result == null)
                        {
                            TableNameAttribute tableNameAttribute;
                            string tableName;
                            if (typeof(T).TryGetAttribute(out tableNameAttribute))
                                tableName = tableNameAttribute.TableName;
                            else
                                tableName = typeof(T).Name;
                            return BadRequest(string.Format(PrimaryKeyField + " provided does not exist in '{0}'.", tableName));
                        }
                        else
                            return Ok(JsonConvert.SerializeObject(result));
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }
            }
            else
            {
                return Unauthorized();
            }

        }


        [HttpPost, Route("Add")]
        public virtual IHttpActionResult Post([FromBody] JObject record)
        {
            if (ViewOnly)
                return Unauthorized();

            try
            {
                if (PostRoles == string.Empty || User.IsInRole(PostRoles))
                {
                    using (AdoDataConnection connection = new AdoDataConnection(Connection))
                    {

                        T newRecord = record.ToObject<T>();
                        int result = new TableOperations<T>(connection).AddNewRecord(newRecord);
                        if (HasUniqueKey)
                        {
                            PropertyInfo prop = typeof(T).GetProperty(UniqueKeyField);
                            if (prop != null)
                            {
                                object uniqueKey = prop.GetValue(newRecord);
                                newRecord = new TableOperations<T>(connection).QueryRecordWhere(UniqueKeyField + " = {0}", uniqueKey);
                                return Ok(JsonConvert.SerializeObject(newRecord));
                            }

                        }
                        return Ok(result);
                    }
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPatch, Route("Update")]
        public virtual IHttpActionResult Patch([FromBody] T record)
        {
            if (ViewOnly)
                return Unauthorized();

            try
            {
                if (PatchRoles == string.Empty || User.IsInRole(PatchRoles))
                {

                    using (AdoDataConnection connection = new AdoDataConnection(Connection))
                    {
                        int result = new TableOperations<T>(connection).AddNewOrUpdateRecord(record);
                        return Ok(result);
                    }
                }
                else
                {
                    return Unauthorized();
                }


            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete, Route("Delete")]
        public virtual IHttpActionResult Delete(T record)
        {
            if (ViewOnly)
                return Unauthorized();

            try
            {
                if (DeleteRoles == string.Empty || User.IsInRole(DeleteRoles))
                {

                    using (AdoDataConnection connection = new AdoDataConnection(Connection))
                    {
                        TableNameAttribute tableNameAttribute;
                        string tableName;
                        if (typeof(T).TryGetAttribute(out tableNameAttribute))
                            tableName = tableNameAttribute.TableName;
                        else
                            tableName = typeof(T).Name;

                        PropertyInfo idProp = typeof(T).GetProperty(PrimaryKeyField);
                        if (idProp.PropertyType == typeof(int))
                        {
                            int id = (int)idProp.GetValue(record);
                            int result = connection.ExecuteNonQuery($"EXEC UniversalCascadeDelete '{tableName}', '{PrimaryKeyField} = {id}'");
                            return Ok(result);

                        }
                        else if (idProp.PropertyType == typeof(Guid))
                        {
                            Guid id = (Guid)idProp.GetValue(record);
                            int result = connection.ExecuteNonQuery($"EXEC UniversalCascadeDelete '{tableName}', '{PrimaryKeyField} = ''{id}'''");
                            return Ok(result);

                        }
                        else if (idProp.PropertyType == typeof(string))
                        {
                            string id = (string)idProp.GetValue(record);
                            int result = connection.ExecuteNonQuery($"EXEC UniversalCascadeDelete '{tableName}', '{PrimaryKeyField} = ''{id}'''");
                            return Ok(result);

                        }
                        else
                        {
                            int result = new TableOperations<T>(connection).DeleteRecord(record);
                            return Ok(result);
                        }
                    }
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route("SearchableList")]
        public virtual IHttpActionResult GetSearchableList([FromBody] PostData postData)
        {
            if (!AllowSearch || (GetRoles != string.Empty && !User.IsInRole(GetRoles)))
                return Unauthorized();

            try
            {

                string whereClause = BuildWhereClause(postData.Searches);

                using (AdoDataConnection connection = new AdoDataConnection(Connection))
                {
                    string tableName = new TableOperations<T>(connection).TableName;

                    string sql = $@"
                    DECLARE @SQLStatement NVARCHAR(MAX) = N'
                        SELECT * FROM {tableName}
                        {whereClause.Replace("'", "''")}
                        ORDER BY { postData.OrderBy} {(postData.Ascending ? "ASC" : "DESC")}
                    '
                    exec sp_executesql @SQLStatement";
                    
                    DataTable table = connection.RetrieveData(sql, "");

                    return Ok(JsonConvert.SerializeObject(table));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion

        #region [Helper Methods]

        protected string BuildWhereClause(IEnumerable<Search> searches)
        {

            string whereClause = string.Join(" AND ", searches.Select(search => {
                if (search.SearchText == string.Empty) search.SearchText = "%";
                else search.SearchText = search.SearchText.Replace("*", "%");

                if (search.Type == "string" || search.Type == "datetime")
                    search.SearchText = $"'{search.SearchText}'";
                else if (Array.IndexOf(new[] { "integer", "number", "boolean" }, search.Type) < 0)
                {
                    string text = search.SearchText.Replace("(", "").Replace(")", "");
                    List<string> things = text.Split(',').ToList();
                    things = things.Select(t => $"'{t}'").ToList();
                    search.SearchText = $"({string.Join(",", things)})";
                }

                return $"{search.FieldName} {search.Operator} {search.SearchText}";
            }));

            if (searches.Any())
                whereClause = "WHERE \n" + whereClause;

            return whereClause;
        }
        #endregion
    }
}