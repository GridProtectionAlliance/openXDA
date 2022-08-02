//******************************************************************************************************
//  EventEmailProcessor.cs - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  02/13/2021 - Stephen C. Wills
//       Generated original version of source code.
//  11/15/2021 - C. Lackner
//       Overhaul of Email Engine to be more modular.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GSF.Data;
using log4net;
using openXDA.Model;

namespace openXDA.Nodes.Types.Email
{
    internal class EventEmailProcessor
    {
        #region [ Members ]

        // Constants
        private const int Idle = 0;
        private const int TimerRunning = 1;

        // Fields
        private EmailType m_emailModel;
        private Func<AdoDataConnection> m_connectionFactory;
        private Action<EmailType, List<Event>> m_sendEmailCallback;

        private ConcurrentQueue<Event> m_triggeredEvents;
        private Action m_skipMaxDelayAction;
        private int m_state;

        #endregion

        #region [ Constructors ]

        public EventEmailProcessor(Action<EmailType, List<Event>> sendEmailCallback)
        {
            m_triggeredEvents = new ConcurrentQueue<Event>();
            m_sendEmailCallback = sendEmailCallback;
        }

        #endregion

        #region [ Properties ]

        public EmailType EmailModel
        {
            get => Interlocked.CompareExchange(ref m_emailModel, null, null);
            set => Interlocked.Exchange(ref m_emailModel, value);
        }

        public Func<AdoDataConnection> ConnectionFactory
        {
            get => Interlocked.CompareExchange(ref m_connectionFactory, null, null);
            set => Interlocked.Exchange(ref m_connectionFactory, value);
        }

        public int EmailTypeID => EmailModel.ID;

        #endregion

        #region [ Methods ]

        public void Process(IEnumerable<Event> events)
        {
            foreach (Event evt in events)
            {
                if (!TriggersEmail(evt))
                    continue;

                m_triggeredEvents.Enqueue(evt);
            }

            _ = StartTimerAsync();
        }

        public void SkipMaxDelayTimer()
        {
            Action skipAction = Interlocked.CompareExchange(ref m_skipMaxDelayAction, null, null);
            skipAction?.Invoke();
        }

        private async Task StartTimerAsync()
        {
            while (!m_triggeredEvents.IsEmpty)
            {
                int previousState = Interlocked.CompareExchange(ref m_state, TimerRunning, Idle);

                if (previousState != Idle)
                    return;

                try { await RunTimerAsync(); }
                catch (Exception ex) { HandleException(ex); }
                finally { Interlocked.Exchange(ref m_state, Idle); }
            }
        }

        private async Task RunTimerAsync()
        {
            DateTime timeStarted = DateTime.UtcNow;
            TimeSpan minDelay = TimeSpan.FromSeconds(EmailModel.MinDelay);
            await Task.Delay(minDelay);

            CancellationTokenSource maxTimerTokenSource = new CancellationTokenSource();
            CancellationToken maxTimerToken = maxTimerTokenSource.Token;

            try
            {
                void SkipMaxTimer()
                {
                    CancellationTokenSource tokenSource = Interlocked.Exchange(ref maxTimerTokenSource, null);

                    if (tokenSource is null)
                        return;

                    tokenSource.Cancel();
                    tokenSource.Dispose();
                }

                Interlocked.Exchange(ref m_skipMaxDelayAction, SkipMaxTimer);

                if (IsAnalysisRunning())
                {
                    TimeSpan maxDelay = TimeSpan.FromSeconds(EmailModel.MaxDelay);
                    TimeSpan elapsed = DateTime.UtcNow - timeStarted;
                    TimeSpan diff = maxDelay - elapsed;

                    try { await Task.Delay(diff, maxTimerToken); }
                    catch (TaskCanceledException) { }
                }
            }
            finally
            {
                Interlocked.Exchange(ref maxTimerTokenSource, null)?.Dispose();
                Interlocked.Exchange(ref m_skipMaxDelayAction, null);
            }

            ExecuteSendEmailCallback();
        }

        private void ExecuteSendEmailCallback()
        {
            List<Event> triggeredEvents = new List<Event>();

            while (m_triggeredEvents.TryDequeue(out Event evt))
                triggeredEvents.Add(evt);

            if (triggeredEvents.Any())
                m_sendEmailCallback(EmailModel, triggeredEvents);
        }

        private bool TriggersEmail(Event evt)
        {
            if ((object)ConnectionFactory is null)
                throw new InvalidOperationException("ConnectionFactory is undefined");

            using (AdoDataConnection connection = ConnectionFactory())
            {
                return connection.ExecuteScalar<bool>(m_emailModel.TriggerEmailSQL, evt.ID);
            }
        }

        private bool IsAnalysisRunning()
        {
            if ((object)ConnectionFactory is null)
                throw new InvalidOperationException("ConnectionFactory is undefined");

            using (AdoDataConnection connection = ConnectionFactory())
            {
                const string Query = "SELECT COUNT(*) FROM AnalysisTask";
                int analysisTaskCount = connection.ExecuteScalar<int>(Query);
                return analysisTaskCount > 0;
            }
        }

        private void HandleException(Exception ex) =>
            Log.Error(ex.Message, ex);

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(EventEmailProcessor));

        #endregion
    }
}
