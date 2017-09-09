//******************************************************************************************************
//  FileSkippedExceptionFilter.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
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
//  09/03/2017 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using log4net.Core;
using log4net.Filter;

namespace openXDA.Logging
{
    public class FileSkippedExceptionFilter : FilterSkeleton
    {
        #region [ Members ]

        // Fields
        private bool m_excludeFileSkippedExceptions;
        private bool m_excludeEverythingElse;

        #endregion

        #region [ Constructors ]

        public FileSkippedExceptionFilter()
            : this(true, false)
        {
        }

        public FileSkippedExceptionFilter(bool excludeFileSkippedExceptions)
            : this(excludeFileSkippedExceptions, !excludeFileSkippedExceptions)
        {
        }

        public FileSkippedExceptionFilter(bool excludeFileSkippedExceptions, bool excludeEverythingElse)
        {
            m_excludeFileSkippedExceptions = excludeFileSkippedExceptions;
            m_excludeEverythingElse = excludeEverythingElse;
        }

        #endregion

        #region [ Methods ]

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            Exception ex = loggingEvent.ExceptionObject ?? DefaultException;
            bool fileSkipped = ex is FileSkippedException;

            if (m_excludeFileSkippedExceptions && fileSkipped)
                return FilterDecision.Deny;

            if (!m_excludeEverythingElse && !fileSkipped)
                return FilterDecision.Deny;

            return FilterDecision.Neutral;
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly Exception DefaultException = new Exception();

        #endregion
    }
}