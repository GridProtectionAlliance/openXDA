//******************************************************************************************************
// DisturbanceFileInfo.cs
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

using System;

namespace FaultAlgorithms
{
    /// <summary>
    /// Represents a disturbance file that was processed by the openFLE.
    /// </summary>
    public class DisturbanceFile
    {
        #region [ Members ]

        // Fields
        private string m_sourcePath;
        private string m_destinationPath;
        private int m_fileSize;
        private DateTime m_creationTime;
        private DateTime m_lastWriteTime;
        private DateTime m_lastAccessTime;
        private DateTime m_fileWatcherTimeStarted;
        private DateTime m_fileWatcherTimeProcessed;
        private DateTime m_fleTimeStarted;
        private DateTime m_fleTimeProcessed;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the path from where the file came.
        /// </summary>
        public string SourcePath
        {
            get
            {
                return m_sourcePath;
            }
            set
            {
                m_sourcePath = value;
            }
        }

        /// <summary>
        /// Gets or sets the path to where the file was dropped.
        /// </summary>
        public string DestinationPath
        {
            get
            {
                return m_destinationPath;
            }
            set
            {
                m_destinationPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        public int FileSize
        {
            get
            {
                return m_fileSize;
            }
            set
            {
                m_fileSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the time the file was created.
        /// </summary>
        public DateTime CreationTime
        {
            get
            {
                return m_creationTime;
            }
            set
            {
                m_creationTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the time the file was last written to.
        /// </summary>
        public DateTime LastWriteTime
        {
            get
            {
                return m_lastWriteTime;
            }
            set
            {
                m_lastWriteTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the time the file was last accessed.
        /// </summary>
        public DateTime LastAccessTime
        {
            get
            {
                return m_lastAccessTime;
            }
            set
            {
                m_lastAccessTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the time that the file watcher started processing the disturbance file.
        /// </summary>
        public DateTime FileWatcherTimeStarted
        {
            get
            {
                return m_fileWatcherTimeStarted;
            }
            set
            {
                m_fileWatcherTimeStarted = value;
            }
        }

        /// <summary>
        /// Gets or sets the time that the file watcher
        /// finished processing the disturbance file.
        /// </summary>
        public DateTime FileWatcherTimeProcessed
        {
            get
            {
                return m_fileWatcherTimeProcessed;
            }
            set
            {
                m_fileWatcherTimeProcessed = value;
            }
        }

        /// <summary>
        /// Gets or sets the time that the fault location
        /// engine started processing the disturbance file.
        /// </summary>
        public DateTime FLETimeStarted
        {
            get
            {
                return m_fleTimeStarted;
            }
            set
            {
                m_fleTimeStarted = value;
            }
        }

        /// <summary>
        /// Gets or sets the time that the fault location
        /// engine finished processing the disturbance file.
        /// </summary>
        public DateTime FLETimeProcessed
        {
            get
            {
                return m_fleTimeProcessed;
            }
            set
            {
                m_fleTimeProcessed = value;
            }
        }

        #endregion
    }
}
