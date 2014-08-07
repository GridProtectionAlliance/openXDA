//******************************************************************************************************
//  MeterDataProcessor.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
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
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  04/30/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using GSF;
using GSF.IO;
using FaultData.DataReaders;

namespace FaultData
{
    public class MeterDataProcessor : IDisposable
    {
        #region [ Members ]

        // Nested Types

        // Constants
        public const string DefaultTrackedDirectory = "Drop";
        private static readonly Guid FileProcessorID = new Guid("{27DAD5B6-7DEC-4B9F-A0BC-87F8DB4C03E2}");

        // Delegates

        // Events
        public event EventHandler<EventArgs<string, List<MeterDataSet>>> Processed;
        public event EventHandler<EventArgs<Exception>> ProcessException;

        // Fields
        private string m_trackedDirectory;
        private FileProcessor m_fileProcessor;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        public MeterDataProcessor()
        {
            m_trackedDirectory = FilePath.GetAbsolutePath(DefaultTrackedDirectory);
            m_fileProcessor = new FileProcessor(FileProcessorID);

            m_fileProcessor.Filter = "*.pqd";
            m_fileProcessor.Processing += FileProcessor_Processing;
            m_fileProcessor.Error += FileProcessor_Error;
        }

        #endregion

        #region [ Properties ]

        public string TrackedDirectory
        {
            get
            {
                return m_trackedDirectory;
            }
            set
            {
                if ((object)value != null)
                    m_trackedDirectory = FilePath.GetAbsolutePath(value);
                else
                    m_trackedDirectory = FilePath.GetAbsolutePath(DefaultTrackedDirectory);
            }
        }

        #endregion

        #region [ Methods ]

        public void Start()
        {
            if (!Directory.Exists(m_trackedDirectory))
                Directory.CreateDirectory(m_trackedDirectory);

            m_fileProcessor.AddTrackedDirectory(m_trackedDirectory);
        }

        public void Stop()
        {
            m_fileProcessor.ClearTrackedDirectories();
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                try
                {
                    if ((object)m_fileProcessor != null)
                    {
                        m_fileProcessor.Dispose();
                        m_fileProcessor = null;
                    }
                }
                finally
                {
                    m_disposed = true;
                }
            }
        }

        private void FileProcessor_Processing(object sender, FileProcessorEventArgs args)
        {
            List<MeterDataSet> meterDataSets;

            if (args.AlreadyProcessed)
                return;

            if ((object)Processed != null)
            {
                meterDataSets = PQDIFReader.ParseFile(args.FullPath);
                Processed(this, new EventArgs<string, List<MeterDataSet>>(args.FullPath, meterDataSets));
            }
        }

        private void FileProcessor_Error(object sender, ErrorEventArgs args)
        {
            OnProcessException(args.GetException());
        }

        private void OnProcessException(Exception ex)
        {
            if ((object)ProcessException != null)
                ProcessException(this, new EventArgs<Exception>(ex));
        }

        #endregion

        #region [ Operators ]

        #endregion

        #region [ Static ]

        // Static Fields

        // Static Constructor

        // Static Properties

        // Static Methods

        #endregion
    }
}
