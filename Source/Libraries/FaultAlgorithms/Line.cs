//******************************************************************************************************
// Line.cs
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  --------------------------------------------------------------------------------------------------- 
//  Portions of this work are derived from "openFLE" which is an Electric Power Research Institute, Inc.
//  (EPRI) copyrighted open source software product released under the BSD license.  openFLE carries
//  the following copyright notice: Version 1.0 - Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC.
//  All rights reserved.
//  ---------------------------------------------------------------------------------------------------
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  03/21/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System.Collections.Generic;
using System.Xml.Linq;

namespace FaultAlgorithms
{
    /// <summary>
    /// Represents a power line between two substations.
    /// </summary>
    public class Line
    {
        #region [ Members ]

        // Fields
        private string m_id;
        private string m_name;
        private string m_voltage;
        private double m_rating50F;
        private double m_length;
        private string m_endStationID;
        private string m_endStationName;

        private double m_r1;
        private double m_x1;
        private double m_r0;
        private double m_x0;

        private FaultTriggerAlgorithm m_faultTriggerAlgorithm;
        private FaultTypeAlgorithm m_faultTypeAlgorithm;
        private FaultLocationAlgorithm m_faultLocationAlgorithm;
        private string m_faultTriggerParameters;
        private string m_faultTypeParameters;
        private string m_faultLocationParameters;

        private XElement m_channelsElement;
        private FaultAlgorithmsSet m_faultAlgorithmsSet;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the ID as defined in the device definitions file.
        /// </summary>
        public string ID
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the line.
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        /// <summary>
        /// Gets or sets the voltage of the line.
        /// </summary>
        public string Voltage
        {
            get
            {
                return m_voltage;
            }
            set
            {
                m_voltage = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum rated current at 50 degrees fahrenheit.
        /// </summary>
        public double Rating50F
        {
            get
            {
                return m_rating50F;
            }
            set
            {
                m_rating50F = value;
            }
        }

        /// <summary>
        /// Gets or sets the length of the line.
        /// </summary>
        public double Length
        {
            get
            {
                return m_length;
            }
            set
            {
                m_length = value;
            }
        }

        /// <summary>
        /// Gets or sets the ID of the station that the line ends at.
        /// </summary>
        public string EndStationID
        {
            get
            {
                return m_endStationID;
            }
            set
            {
                m_endStationID = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the station that the line ends at.
        /// </summary>
        public string EndStationName
        {
            get
            {
                return m_endStationName;
            }
            set
            {
                m_endStationName = value;
            }
        }

        /// <summary>
        /// Gets or sets the real component for the positive sequence impedance.
        /// </summary>
        public double R1
        {
            get
            {
                return m_r1;
            }
            set
            {
                m_r1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the imaginary component for the positive sequence impedance.
        /// </summary>
        public double X1
        {
            get
            {
                return m_x1;
            }
            set
            {
                m_x1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the real component for the zero sequence impedance.
        /// </summary>
        public double R0
        {
            get
            {
                return m_r0;
            }
            set
            {
                m_r0 = value;
            }
        }

        /// <summary>
        /// Gets or sets the imaginary component for the zero sequence impedance.
        /// </summary>
        public double X0
        {
            get
            {
                return m_x0;
            }
            set
            {
                m_x0 = value;
            }
        }

        /// <summary>
        /// Gets or sets the fault algorithms used for detecting
        /// faults and calculating fault distance on the line.
        /// </summary>
        public FaultAlgorithmsSet FaultAlgorithmsSet
        {
            get
            {
                return m_faultAlgorithmsSet;
            }
            set
            {
                m_faultAlgorithmsSet = value;
            }
        }

        /// <summary>
        /// XML element that contains channel mappings for the ComtradeLoader.
        /// </summary>
        public XElement ChannelsElement
        {
            get
            {
                return m_channelsElement;
            }
            set
            {
                m_channelsElement = value;
            }
        }

        #endregion
    }
}
