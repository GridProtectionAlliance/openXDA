//******************************************************************************************************
//  RollingFileTraceListener.cs - Gbtc
//
//  Copyright © 2025, Grid Protection Alliance.  All Rights Reserved.
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
//  09/30/2025 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using HIDS;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;

namespace openXDA.Logging
{
    public class RollingFileTraceListener : TraceListener
    {
        #region [ Constructors ]

        public RollingFileTraceListener()
            : this(@"Debug\Trace.log")
        {
        }

        public RollingFileTraceListener(string logPath)
        {
            LazyAppender = new Lazy<RollingFileAppender>(() =>
            {
                RollingFileAppender appender = new RollingFileAppender();
                appender.File = logPath;
                appender.StaticLogFileName = false;
                appender.AppendToFile = true;
                appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
                appender.MaxSizeRollBackups = 10;
                appender.PreserveLogFileNameExtension = true;
                appender.MaximumFileSize = "1MB";
                appender.Layout = new PatternLayout("%date [%thread] %-5level %logger - %message%newline");
                appender.LockingModel = new FileAppender.NoLock();
                appender.ActivateOptions();
                return appender;
            });
        }

        #endregion

        #region [ Properties ]

        private Lazy<RollingFileAppender> LazyAppender { get; }
        private RollingFileAppender Appender => LazyAppender.Value;

        #endregion

        #region [ Methods ]

        public override void WriteLine(object o, string category) =>
            WriteLine(o);

        public override void WriteLine(object o) =>
            WriteLine(o?.ToString());

        public override void WriteLine(string message, string category) =>
            WriteLine(message);

        public override void WriteLine(string message)
        {
            if (!ShouldTrace(message))
                return;

            TraceMessage(Thread.CurrentThread.Name, TraceEventType.Verbose, message);
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId) =>
            TraceEvent(eventCache, source, TraceEventType.Transfer, id, message);

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (!ShouldTrace(eventCache, source, eventType, id, format, args))
                return;

            string message = string.Format(CultureInfo.InvariantCulture, format, args);
            TraceMessage(eventCache.ThreadId, eventType, message);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (!ShouldTrace(eventCache, source, eventType, id, message, null))
                return;

            TraceMessage(eventCache.ThreadId, eventType, message);
        }

        private bool ShouldTrace(string message) =>
            ShouldTrace(null, string.Empty, TraceEventType.Verbose, 0, message);

        private bool ShouldTrace(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message) =>
            ShouldTrace(eventCache, source, eventType, id, message, null);

        private bool ShouldTrace(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args) =>
            Filter is null || Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null);

        private void TraceMessage(string threadName, TraceEventType eventType, string message)
        {
            LoggingEventData logData = new LoggingEventData()
            {
                LoggerName = Name,
                Level = ToLevel(eventType),
                Message = message,
                ThreadName = threadName
            };

            LoggingEvent evt = new LoggingEvent(typeof(API), null, logData);
            Appender.DoAppend(evt);
        }

        private Level ToLevel(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Critical:
                    return Level.Critical;

                case TraceEventType.Error:
                    return Level.Error;

                case TraceEventType.Warning:
                    return Level.Warn;

                case TraceEventType.Information:
                    return Level.Info;

                case TraceEventType.Verbose:
                    return Level.Trace;

                default:
                case TraceEventType.Start:
                case TraceEventType.Stop:
                case TraceEventType.Suspend:
                case TraceEventType.Resume:
                case TraceEventType.Transfer:
                    return Level.Verbose;
            }
        }

        #region [ Not Supported ]

        public override void Write(string message) { }
        public override void Write(object o) { }
        public override void Write(object o, string category) { }
        public override void Write(string message, string category) { }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data) { }
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data) { }

        #endregion

        #endregion
    }
}