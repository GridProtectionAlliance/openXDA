//******************************************************************************************************
//  CategorizedSetting.cs - Gbtc
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
//  07/31/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace openXDA.Configuration
{
    public class CategorizedSetting
    {
        #region [ Members ]

        // Fields
        private List<string> m_categories;
        private string m_value;

        #endregion

        #region [ Constructors ]

        public CategorizedSetting(string name, string value)
        {
            m_categories = Regex.Replace(name, @"\.+", ".")
                .Split('.')
                .Select(category => category.Trim())
                .ToList();

            m_value = value;
        }

        #endregion

        #region [ Properties ]

        public List<string> Categories
        {
            get
            {
                return m_categories;
            }
        }

        public string Value
        {
            get
            {
                return m_value;
            }
        }

        #endregion
    }
}
