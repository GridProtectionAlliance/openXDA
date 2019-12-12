//******************************************************************************************************
//  ChannelData.cs - Gbtc
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
//  12/12/2019 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************


using GSF.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openXDA.Model.Channels
{
    [TableName("ChannelData")]
    class ChannelData
    {
        [PrimaryKey(true)]
        public int ID { get; set; }

        public int FileGroupID { get; set; }

        public int RunTimeID { get; set; }

        public byte[] TimeDomainData { get; set; }

        // Not sure we need this but it was in EventData
        public int MarkedForDeletion { get; set; }

        public int SeriesID { get; set; }

        public int EventID { get; set; }

        // This is for backwards compatibility so we can point to data that is still in a EventDataBlob.
        // As we pull up the data it will be moved out but if this is the first time Calling the ChannelData Blob
        // it just points back to the eventdata Blob.
        public int? EventDataID {get; set;}

        }
}
