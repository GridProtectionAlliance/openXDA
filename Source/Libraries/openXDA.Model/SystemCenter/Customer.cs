//******************************************************************************************************
//  AdditionalField.cs - Gbtc
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
using System.ComponentModel.DataAnnotations;

namespace SystemCenter.Model
{
    /// <summary>
    /// Customer Model. Distinct and joined to PQViewSiteID to reduce number of duplicate Customers coming from PQView
    /// </summary>
    [AllowSearch]
    [CustomView(@"
    SELECT
        DISTINCT
        Customer.ID,
	    Customer.CustomerKey,
	    Customer.Name,
	    Customer.Phone,
	    Customer.Description,
	    COUNT([CustomerAccess].ID) as Meters
    FROM
	    Customer LEFT JOIN
	    CustomerAccess ON Customer.ID = [CustomerAccess].CustomerID LEFT JOIN
	    PQViewSite ON [CustomerAccess].PQViewSiteID = PQViewSite.ID 
    GROUP BY
        Customer.ID,
	    Customer.CustomerKey,
	    Customer.Name,
	    Customer.Phone,
	    Customer.Description")]
    [PatchRoles("Administrator, Transmission SME")]
    [PostRoles("Administrator, Transmission SME")]
    [DeleteRoles("Administrator, Transmission SME")]
    public class Customer
    {
        [PrimaryKey(true)]
        public int ID { get; set; }
        [Required]
        [DefaultSortOrder]
        public string CustomerKey { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
    }
}