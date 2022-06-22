//******************************************************************************************************
//  D3Series.cs - Gbtc
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
//  08/10/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************
using GSF;
using GSF.Data.Model;
using GSF.Web;
using GSF.Web.Model;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace SEBrowser.Model
{
    /// <summary>
    /// Defines a Data Series that can be plotted on a Graph.
    /// Primary usage in LSCVS for now
    /// </summary>
    /// <remarks>
    /// Will need to use in SEbrowser ans OpenSEE too.
    /// </remarks>
    [Serializable]
    public class D3Series
    {
      

        #region [ Properties ]

        /// <summary>
        /// Label of this Channel
        /// </summary>
        public string ChartLabel = "";
        /// <summary>
        /// Group used to determine buttons on top of the Legend
        /// </summary>
        public string LegendGroup = "";

        /// <summary>
        /// Horizontal Legend Category
        /// </summary>
        public string LegendHorizontal = "";

        /// <summary>
        /// Vertical Legend Category
        /// </summary>
        public string LegendVertical = "";

        /// <summary>
        /// Unit of the Channel
        /// </summary>
        public string Unit = "";

        /// <summary>
        /// Color of the Channel
        /// </summary>
        public string Color = "";

        /// <summary>
        /// Group used to determine vertical category - if applicable
        /// </summary>
        public string LegendVGroup = "";

        /// <summary>
        /// Base Value, can be used for p.u. computations
        /// </summary>
        public double BaseValue;

        /// <summary>
        /// Datapoints 
        /// </summary>
        public List<double[]> DataPoints = new List<double[]>();

        /// <summary>
        /// Markes to indicate specific points - if applicable
        /// </summary>
        public List<double[]> DataMarker = new List<double[]>();

        #endregion
    }
}