//******************************************************************************************************
//  EPRICapBankAnalytics.cs - Gbtc
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
//  07/29/2020 - Christoph Lackner
//       Generated original version of source code.
//
//******************************************************************************************************


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataOperations;
using FaultData.DataSets;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;
using log4net;
using System.IO;
using System.Collections.Concurrent;
using GSF.Threading;
using System.Threading;

namespace FaultData.DataOperations
{
    public class EPRICapBankAnalytics
    {
        #region [Members]

        // Constants
        private const int DoNothingAction = 0;
        private const int StartTimerAction = 1;
        private const int StopTimerAction = 2;

        // Fields
        private List<Event> m_triggeredEvents;
        private int m_fileGroupID;
        private Action<List<Event>, int> m_processEventCallback;

        private ISynchronizedOperation m_synchronizedOperation;
        private ICancellationToken m_delayCancellationToken;

        private int m_timerAction;
        private int m_runAnalysis;


        #endregion

        #region [Constructors]

        public EPRICapBankAnalytics(Action<List<Event>, int> processCallback)
        {
            m_triggeredEvents = new List<Event>();
            m_fileGroupID = -1;
            m_processEventCallback = processCallback;
            m_synchronizedOperation = new LongSynchronizedOperation(UpdateTimersAndRunAnalysis, HandleException);
        }

        #endregion

        #region [Methods]

        public void StartTimer()
        {
            Interlocked.Exchange(ref m_timerAction, StartTimerAction);
            m_synchronizedOperation.Run();
        }

        public void StopTimer()
        {
            Interlocked.Exchange(ref m_timerAction, StopTimerAction);
            m_synchronizedOperation.Run();
        }

        private void RunAnalysis()
        {
            Interlocked.Exchange(ref m_runAnalysis, 1);
            m_synchronizedOperation.Run();
        }

        // Actions are synchronized to prevent race
        // conditions when starting and stopping timers.
        private void UpdateTimersAndRunAnalysis()
        {
            int timerAction = Interlocked.Exchange(ref m_timerAction, DoNothingAction);
            int runAnalysis = Interlocked.Exchange(ref m_runAnalysis, 0);

            switch (timerAction)
            {
                case StartTimerAction:
                    StartDelayTimer();
                    break;

                case StopTimerAction:
                    StopDelayTimer();
                    break;
            }
          
            if (runAnalysis != 0)
                ExecuteProcessCallback();
        }


        private void StopDelayTimer()
        {
            ICancellationToken cancellationToken = Interlocked.Exchange(ref m_delayCancellationToken, null);
            cancellationToken?.Cancel();
        }

        private void StartDelayTimer()
        {
            if (m_triggeredEvents.Count() == 0)
                return;

            ICancellationToken cancellationToken = Interlocked.CompareExchange(ref m_delayCancellationToken, null, null);

            // Check if the timer has already been started
            if ((object)cancellationToken != null)
                return;

            Action runAnalysisAction = new Action(RunAnalysis);
            int delay = Delay;
            cancellationToken = runAnalysisAction.DelayAndExecute(delay);
            Interlocked.Exchange(ref m_delayCancellationToken, cancellationToken);
        }

        private void ExecuteProcessCallback()
        {
            ICancellationToken delayCancellationToken = Interlocked.Exchange(ref m_delayCancellationToken, null);
            delayCancellationToken?.Cancel();

            List<Event> triggeredEvents = Interlocked.Exchange(ref m_triggeredEvents, null);
            int fileGroupId = Interlocked.Exchange(ref m_fileGroupID, -1);

            if (triggeredEvents.Any())
                m_processEventCallback(triggeredEvents, fileGroupId);
        }

        private void HandleException(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }


        public void Process(List<Event> events, int fileGroupID)
        {
            Interlocked.Exchange(ref m_triggeredEvents, events);
            Interlocked.Exchange(ref m_fileGroupID, fileGroupID);

            m_synchronizedOperation.Run();
        }

        #endregion

     
        #region[ Static ]

        private static readonly ILog Log = LogManager.GetLogger(typeof(EPRICapBankAnalytics));

        private static readonly int Delay = 10000;
        #endregion
    }

}
