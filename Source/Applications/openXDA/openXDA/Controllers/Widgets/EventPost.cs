//******************************************************************************************************
//  EventPost.tsx - Gbtc
//
//  Copyright © 2025, Grid Protection Alliance.  All Rights Reserved.
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
//  12/26/2020 - Gabriel Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;
using Customer = SystemCenter.Model.Customer;

namespace openXDA.Controllers.Widgets
{
    public class EventPost
    {
        /// <summary>
        /// Represents the ID of an XDA <see cref="Event"/>.
        /// </summary>
        public int EventID { get; set; }
        /// <summary>
        /// Represents the key of an XDA <see cref="Customer"/>.
        /// </summary>
        /// <remarks>This value may be <see langword="null"/>, this represents complete access to the XDA database.</remarks>
        public string CustomerKey { get; set; }

        /// <summary>
        /// Tells if the <see cref="Customer"/> defined in this object is auhorized to view the <see cref="Event"/> defined in this object.
        /// </summary>
        /// <param name="connection">The <see cref="AdoDataConnection"/> that performs lookups.</param>
        /// <returns>A <see langword="bool"/> that represents authorization success.</returns>
        public bool IsCustomerAuthorized(AdoDataConnection connection)
        {
            if (CustomerKey is null) return true;

            Customer customer = new TableOperations<Customer>(connection).QueryRecord(new RecordRestriction("CustomerKey = {0}", CustomerKey));
            Event evt = new TableOperations<Event>(connection).QueryRecordWhere(@"
                (
                    MeterID in (
                        SELECT MeterID
                        FROM CustomerMeter
                        WHERE CustomerID = {0}
                    ) OR
                    AssetID in (
                        SELECT AssetID
                        FROM CustomerAsset
                        WHERE CustomerID = {0}
                    )
                )
                AND
                ID = {1}
            ", customer.ID, EventID);
            return !(evt is null);
        }

        /// <summary>
        /// Creates a <see cref="RecordRestriction"/> for a <see cref="Channel"/> table.
        /// </summary>
        /// <returns>A <see cref="RecordRestriction"/> that constrains to only records viewable by the <see cref="Customer"/> defined in this object.</returns>
        public RecordRestriction GetCustomerRestrictionOnChannels()
        {
            if (CustomerKey is null) return null;

            return new RecordRestriction(
                @"MeterID IN (
                    SELECT MeterID FROM CustomerMeter WHERE CustomerID = 
                        (SELECT ID FROM Customer WHERE CustomerKey = {0})
                ) OR 
                AssetID IN (
                    SELECT MeterID FROM CustomerMeter WHERE CustomerID = 
                        (SELECT ID FROM Customer WHERE CustomerKey = {0})
                )",
                CustomerKey
            );
        }
    }
}
