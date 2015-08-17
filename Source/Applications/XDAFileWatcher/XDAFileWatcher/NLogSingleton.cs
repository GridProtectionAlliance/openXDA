//  NLogSingleton.cs - Gbtc
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

// see: http://nlog-project.org/ for more information
// integrated with Harvester Ver. 2.0 (http://harvester.codeplex.com/) for real-time viewing for debug

// note an alernative to this static class is to create an instance of NLog within each Class requiring logging
// this approach provides class granularity on log messaging and can be used in combination with this static class

using NLog;

namespace XDAFileWatcher
{
    internal static class Log
    {

        public static Logger Instance { get; private set; }
        static Log()
        {

// Not sure yet how to switch the piped stream to Harvester for VS 2012.  Code below is for VS 2010.
// See: NLog.config

//#if DEBUG
//            var harvesterTarget = new OutputDebugStringTarget() { Name = "harvester", Layout = new Log4JXmlEventLayout() };
//              
//             var harvesterRule = new LoggingRule("*", LogLevel.Info, harvesterTarget);


//            LogManager.Configuration.AddTarget("harvester", harvesterTarget);
//            LogManager.Configuration.LoggingRules.Add(harvesterRule);

////#else
           
////            var defaultRule = new LoggingRule("*".LogLevel.Warn, defaultTarget);

//#endif

//            LogManager.Configuration.LoggingRules.Add(harvesterRule2);
//            LogManager.ReconfigExistingLoggers();

            
            Instance = LogManager.GetCurrentClassLogger();

            Instance.Info("NLog Loaded");

            //Instance.Trace("Sample trace message");
            //Instance.Debug("Sample debug message");
            //Instance.Info("Sample informational message");
            //Instance.Warn("Sample warning message");
            //Instance.Error("Sample error message");
            //Instance.Fatal("Sample fatal error message");
        }

        public static void FlushLog()
        {
         LogManager.Flush();
        }
      
    }
}
