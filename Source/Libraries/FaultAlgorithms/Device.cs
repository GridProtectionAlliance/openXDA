//*********************************************************************************************************************
// Device.cs
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
//  -------------------------------------------------------------------------------------------------------------------
//  03/21/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System.Collections.Generic;

namespace FaultAlgorithms
{
    /// <summary>
    /// Represents a device that produces event files.
    /// </summary>
    public class Device
    {
        #region [ Members ]

        // Fields
        private string m_id;
        private string m_make;
        private string m_model;
        private string m_stationID;
        private string m_stationName;
        private readonly List<Line> m_lines;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device()
        {
            m_lines = new List<Line>();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the ID of the device as defined in the device definitions file.
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
        /// Gets or sets the make of the device.
        /// </summary>
        public string Make
        {
            get
            {
                return m_make;
            }
            set
            {
                m_make = value;
            }
        }

        /// <summary>
        /// Gets or sets the model of the device.
        /// </summary>
        public string Model
        {
            get
            {
                return m_model;
            }
            set
            {
                m_model = value;
            }
        }

        /// <summary>
        /// Gets or sets the ID of the station where the device is located.
        /// </summary>
        public string StationID
        {
            get
            {
                return m_stationID;
            }
            set
            {
                m_stationID = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the station where the device is located.
        /// </summary>
        public string StationName
        {
            get
            {
                return m_stationName;
            }
            set
            {
                m_stationName = value;
            }
        }

        /// <summary>
        /// Gets the lines that the device is monitoring.
        /// </summary>
        public ICollection<Line> Lines
        {
            get
            {
                return m_lines;
            }
        }

        #endregion
    }
}
