//******************************************************************************************************
//  ServiceHelperAppender.cs - Gbtc
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
//  02/27/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF;
using GSF.ServiceProcess;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace openXDA.Logging
{
    public class ServiceHelperAppender : IAppender
    {
        #region [ Members ]

        // Fields
        private string m_name;
        private ServiceHelper m_serviceHelper;

        #endregion

        #region [ Constructors ]

        public ServiceHelperAppender(ServiceHelper serviceHelper)
        {
            m_serviceHelper = serviceHelper;
            m_serviceHelper.Disposed += (sender, args) => m_serviceHelper = null;
        }

        #endregion

        #region [ Properties ]

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

        #endregion

        #region [ Methods ]

        public void DoAppend(LoggingEvent loggingEvent)
        {
            object threadID;
            string renderedMessage;
            Exception ex;
            UpdateType updateType;

            // If the service helper has been disposed,
            // do not log the event
            if ((object)m_serviceHelper == null)
                return;

            // Determine the update type based on the log level
            if (loggingEvent.Level.Value >= Level.Error.Value)
                updateType = UpdateType.Alarm;
            else if (loggingEvent.Level.Value >= Level.Warn.Value)
                updateType = UpdateType.Warning;
            else if (loggingEvent.Level.Value >= Level.Info.Value)
                updateType = UpdateType.Information;
            else
                return;

            // Determine the thread ID from the thread's context
            threadID = ThreadContext.Properties["ID"] ?? "0";

            // Get the message and exception object
            renderedMessage = loggingEvent.RenderedMessage;
            ex = loggingEvent.ExceptionObject;

            // If the user didn't supply a message,
            // attempt to use the exception message instead
            if (string.IsNullOrEmpty(renderedMessage))
                renderedMessage = loggingEvent.GetExceptionString();

            // If the event was an exception event,
            // also log to the service helper's error log
            if ((object)ex != null)
            {
                if ((object)m_serviceHelper.ErrorLogger != null)
                    m_serviceHelper.ErrorLogger.Log(ex);

                if (string.IsNullOrEmpty(renderedMessage))
                    renderedMessage = ex.Message;
            }

            // Send the message to clients via the service helper
            if (!string.IsNullOrEmpty(renderedMessage))
                m_serviceHelper.UpdateStatusAppendLine(updateType, "[{0}] {1}", threadID, renderedMessage);
            else
                m_serviceHelper.UpdateStatusAppendLine(updateType, "");
        }

        public void Close()
        {
        }

        #endregion
    }
}
