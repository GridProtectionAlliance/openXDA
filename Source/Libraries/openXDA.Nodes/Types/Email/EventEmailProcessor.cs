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
using GSF.Data;
using GSF.Threading;
using log4net;
using openXDA.Model;

namespace openXDA.Nodes.Types.Email
{
    internal class EventEmailProcessor
    {
        #region [ Members ]

        // Constants
        private const int DoNothingAction = 0;
        private const int StartTimerAction = 1;
        private const int StopTimerAction = 2;

        // Fields
        private EmailType m_emailModel;
        private Func<AdoDataConnection> m_connectionFactory;
        private ConcurrentQueue<Event> m_triggeredEvents;
        private Action<EmailType, List<Event>> m_sendEmailCallback;

        private ISynchronizedOperation m_synchronizedOperation;
        private ICancellationToken m_minDelayCancellationToken;
        private ICancellationToken m_maxDelayCancellationToken;
        private int m_timerAction;
        private int m_sendEmail;

        #endregion

        #region [ Constructors ]

        public EventEmailProcessor(Action<EmailType, List<Event>> sendEmailCallback)
        {
            m_triggeredEvents = new ConcurrentQueue<Event>();
            m_sendEmailCallback = sendEmailCallback;
            m_synchronizedOperation = new LongSynchronizedOperation(UpdateTimersAndSendEmail, HandleException) { IsBackground = true };
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

        public void Process(Event evt)
        {
            if (EventTrigger(evt))
            {
                m_triggeredEvents.Enqueue(evt);
                m_synchronizedOperation.Run();
            }
        }

        /// <summary>
        /// Determine Whether the Email should be Triggered based on SQL
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        private bool EventTrigger(Event evt)
        {
            if ((object)ConnectionFactory == null)
                throw new InvalidOperationException("ConnectionFactory is undefined");

            using (AdoDataConnection connection = ConnectionFactory())
            {
                return connection.ExecuteScalar<bool>(m_emailModel.TriggerEmailSQL, evt.ID);
            }
        }

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

        private void SendEmail()
        {
            Interlocked.Exchange(ref m_sendEmail, 1);
            m_synchronizedOperation.Run();
        }

        // Actions are synchronized to prevent race
        // conditions when starting and stopping timers.
        private void UpdateTimersAndSendEmail()
        {
            int timerAction = Interlocked.Exchange(ref m_timerAction, DoNothingAction);
            int sendEmail = Interlocked.Exchange(ref m_sendEmail, 0);

            switch (timerAction)
            {
                case StartTimerAction:
                    StartMinDelayTimer();
                    break;

                case StopTimerAction:
                    StopMinDelayTimer();
                    break;
            }

            StartMaxDelayTimer();

            if (sendEmail != 0)
                ExecuteSendEmailCallback();
        }

        private void StartMinDelayTimer()
        {
            if (m_triggeredEvents.IsEmpty)
                return;

            ICancellationToken cancellationToken = Interlocked.CompareExchange(ref m_minDelayCancellationToken, null, null);

            // Check if the timer has already been started
            if ((object)cancellationToken != null)
                return;

            Action sendEmailAction = new Action(SendEmail);
            int delay = (int)Math.Round(TimeSpan.FromSeconds(EmailModel.MinDelay).TotalMilliseconds);
            cancellationToken = sendEmailAction.DelayAndExecute(delay);
            Interlocked.Exchange(ref m_minDelayCancellationToken, cancellationToken);
        }

        private void StopMinDelayTimer()
        {
            ICancellationToken cancellationToken = Interlocked.Exchange(ref m_minDelayCancellationToken, null);
            cancellationToken?.Cancel();
        }

        private void StartMaxDelayTimer()
        {
            if (m_triggeredEvents.IsEmpty)
                return;

            ICancellationToken cancellationToken = Interlocked.CompareExchange(ref m_maxDelayCancellationToken, null, null);

            // Check if the timer has already been started
            if ((object)cancellationToken != null)
                return;

            Action sendEmailAction = new Action(SendEmail);
            int delay = (int)Math.Round(TimeSpan.FromSeconds(EmailModel.MaxDelay).TotalMilliseconds);
            cancellationToken = sendEmailAction.DelayAndExecute(delay);
            Interlocked.Exchange(ref m_maxDelayCancellationToken, cancellationToken);
        }

        private void ExecuteSendEmailCallback()
        {
            ICancellationToken minDelayCancellationToken = Interlocked.Exchange(ref m_minDelayCancellationToken, null);
            ICancellationToken maxDelayCancellationToken = Interlocked.Exchange(ref m_maxDelayCancellationToken, null);

            minDelayCancellationToken?.Cancel();
            maxDelayCancellationToken?.Cancel();

            List<Event> triggeredEvents = new List<Event>();

            while (m_triggeredEvents.TryDequeue(out Event evt))
                triggeredEvents.Add(evt);

            if (triggeredEvents.Any())
                m_sendEmailCallback(EmailModel, triggeredEvents);
        }

        private void HandleException(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(EventEmailProcessor));

        #endregion
    }
}
