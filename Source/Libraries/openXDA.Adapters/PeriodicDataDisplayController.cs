//******************************************************************************************************
//  PeriodicDataDisplay1.cs - Gbtc
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
//  05/25/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Web;
using GSF.Web.Model;
using openXDA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace openXDA.Adapters
{
    public class PeriodicDataDisplayController : ApiController
    {
        #region [ Members ]

        // Fields
        private DateTime m_epoch = new DateTime(1970, 1, 1);
        private DataContext m_dataContext;

        #endregion
        #region [ Properties ]
        public DataContext DataContext => m_dataContext ?? (m_dataContext = new DataContext("systemSettings"));
        #endregion

        #region [ Constructors ]
        PeriodicDataDisplayController() {

        }

        ~PeriodicDataDisplayController() {
            m_dataContext?.Dispose();
        }
        #endregion

        #region [ Static ]
        private static MemoryCache s_memoryCache;

        static PeriodicDataDisplayController()
        {
            s_memoryCache = new MemoryCache("PeriodicDataDisplay1");
        }
        #endregion


        #region [ Methods ]
        [HttpGet]
        public dynamic GetData()
        {
            Dictionary<string, string> query = Request.QueryParameters();

            return null;

        }

        [HttpGet]
        public IEnumerable<Meter> GetMeters()
        {
            Dictionary<string, string> query = Request.QueryParameters();

            return DataContext.Table<Meter>().QueryRecords();

        }



        #endregion

    }
}
