//******************************************************************************************************
//  SubscriptionSection.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
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
//  08/02/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.ComponentModel;
using System.Configuration;
using System.Security;
using GSF;

namespace openXDA.Configuration
{
    public class SubscriptionSection
    {
        #region [ Members ]

        // Constants
        public const string CategoryName = "Subscription";

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the Subject Line used when sending the confirmation email
        /// </summary>
        [Setting]
        [DefaultValue("OpenXDA Confirm Email")]
        public string ConfirmSubject { get; set; }

        /// <summary>
        /// Gets or sets the Template used when generating the Confirmation Email
        /// </summary>
        [Setting]
        [DefaultValue("Please click the following Link to confirm your email address \n http://localhost/SystemCenterNotification/ConfirmEmail")]
        public string ConfirmTemplate { get; set; }


        /// <summary>
        /// Gets or sets the flag that determines whether a confirmed Email is Required 
        /// when subscribing to a notification
        /// </summary>
        [Setting]
        [DefaultValue(true)]
        public bool RequireConfirmation { get; set; }

        #endregion
    }
}
