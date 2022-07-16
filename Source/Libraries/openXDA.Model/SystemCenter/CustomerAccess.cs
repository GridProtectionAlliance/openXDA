//******************************************************************************************************
//  CustomerAccess.cs - Gbtc
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
//  09/20/2019 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace SystemCenter.Model
{
    [UseEscapedName, TableName("CustomerMeter")]
    [PostRoles("Administrator, Transmission SME")]
    [PatchRoles("Administrator, Transmission SME")]
    [DeleteRoles("Administrator, Transmission SME")]
    public class CustomerMeter
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        [ParentKey(typeof(Customer))]
        public int CustomerID { get; set; }
        public int MeterID { get; set; }
    }

    [UseEscapedName]
    [PostRoles("Administrator, Transmission SME")]
    [PatchRoles("Administrator, Transmission SME")]
    [DeleteRoles("Administrator, Transmission SME")]
    public class CustomerMeterDetail
    {
        #region [ Members ]

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID { get; set; }

        [Searchable]
        public string CustomerKey { get; set; }

        public string CustomerName { get; set; }

        [Searchable]
        public string MeterKey { get; set; }

        public string MeterName { get; set; }

        public string MeterLocation { get; set; }

        public int MeterID { get; set; }

        [ParentKey(typeof(Customer))]
        public int CustomerID { get; set; }

        #endregion

        #region [ Methods ]      

        #endregion
    }
}