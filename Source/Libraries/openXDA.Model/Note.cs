//******************************************************************************************************
//  Note.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  01/14/2020 - Billy Ernest
//       Generated original version of source code.
//  05/16/2021 - C. Lackner
//       Merged notes model from SystemCenter.
//
//******************************************************************************************************

using GSF.Data;
using GSF.Data.Model;
using GSF.Web.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace openXDA.Model
{
    [TableName("Note"), UseEscapedName]
    [PostRoles("Administrator, Transmission SME, PQ Data Viewer")]
    [DeleteRoles("Administrator, Transmission SME")]
    [PatchRoles("Administrator, Transmission SME")]
    [AllowSearch]
    public class Notes
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        public int NoteTypeID { get; set; }
        public int NoteApplicationID { get; set; } = 2;
        public int NoteTagID { get; set; } = 1;
        public int ReferenceTableID { get; set; }
        public string Note { get; set; }
        public string UserAccount { get; set; }
        [DefaultSortOrder(false)]
        public DateTime Timestamp { get; set; }

    }

    public class NotesController<T>: ModelController<T> where T: Notes, new()
    {
        public override IHttpActionResult Post([FromBody] JObject record)
        {
            try
            {
                if (User.IsInRole(PostRoles))
                {
                    using (AdoDataConnection connection = new AdoDataConnection(Connection))
                    {
                        Notes newRecord = record.ToObject<Notes>();

                        newRecord.UserAccount = User.Identity.Name;
                        int result = new TableOperations<Notes>(connection).AddNewRecord(newRecord);
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

    }
}
