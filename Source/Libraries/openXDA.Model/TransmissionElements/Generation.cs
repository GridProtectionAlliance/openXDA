﻿//******************************************************************************************************
//  Generation.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  12/13/2019 - Christoph Lackner
//      Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

namespace openXDA.Model
{
    [MetadataType(typeof(Asset))]
    public class Generation : Asset
    {
        #region [ Members ]

        #endregion

        #region [ Properties ]

        #endregion

        #region [ Methods ]

        public static Generation DetailedGen(Asset asset, AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Generation> genTable = new TableOperations<Generation>(connection);
            Generation gen = genTable.QueryRecordWhere("ID = {0}", asset.ID);

            if (gen != null)
                gen.LazyContext = asset.LazyContext;

            return gen;
        }

        public static Generation DetailedGen(Asset asset)
        {
            return DetailedGen(asset, asset.ConnectionFactory.Invoke());
        }
        #endregion
    }
}
