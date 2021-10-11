//******************************************************************************************************
//  BreakerConfiguration.cs - Gbtc
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
//  06/25/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FaultData.DataSets;
using GSF;
using log4net;

namespace FaultData.DataOperations.DFRLineFiles
{
    public class BreakerConfiguration
    {
        #region [ Members ]

        // Constants
        private const string ITSNumberQuery = "SELECT ITSOpernum FROM MaximoBreaker WHERE BreakerNum = @breakerNum";

        // Fields
        private LineConfiguration.DigitalInfo[] m_channels;

        #endregion

        #region [ Constructors ]

        public BreakerConfiguration(MeterDataSet meterDataSet, string breakerNumber)
        {
            string itsNumber;
            string breakerIdentifier;

            try
            {
                itsNumber = new string(breakerNumber.Reverse().Take(3).ToArray()).Reverse();

                if (!string.IsNullOrEmpty(itsNumber))
                {
                    breakerIdentifier = string.Format("[A-Z]CB #?{0}", Regex.Escape(itsNumber));

                    m_channels = meterDataSet.Digitals
                        .Select(dataSeries => dataSeries.SeriesInfo?.Channel)
                        .Where(channel => (object)channel != null)
                        .Where(channel => Regex.IsMatch(channel.Description, breakerIdentifier))
                        .Select(channel => new LineConfiguration.DigitalInfo(meterDataSet, channel.Name))
                        .Where(digital => digital.MeasurementCharacteristic != "Unknown")
                        .ToArray();
                }
            }
            catch (Exception ex)
            {
                Log.Warn(ex.Message, ex);
            }

            if ((object)m_channels == null)
                m_channels = new LineConfiguration.DigitalInfo[0];
        }

        #endregion

        #region [ Properties ]

        public LineConfiguration.DigitalInfo[] Channels
        {
            get
            {
                return m_channels;
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private readonly ILog Log = LogManager.GetLogger(typeof(BreakerConfiguration));

        #endregion
    }
}
