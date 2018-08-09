//******************************************************************************************************
//  EventEmailEngine.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  08/04/2018 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Xml.Linq;
using FaultData.DataSets;
using FaultData.DataWriters;
using GSF.Data;
using GSF.Data.Model;
using GSF.Threading;
using openXDA.Model;

namespace openXDA
{
    public class EventEmailEngine
    {
        #region [ Members ]

        // Nested Types

        private class EventEmailType
        {
            #region [ Members ]

            // Constants
            private const int DoNothingAction = 0;
            private const int StartTimerAction = 1;
            private const int StopTimerAction = 2;

            // Fields
            private EventEmailParameters m_parameters;
            private ConcurrentQueue<Event> m_triggeredEvents;
            private Action<EventEmailParameters, List<Event>> m_sendEmailCallback;

            private ISynchronizedOperation m_synchronizedOperation;
            private ICancellationToken m_minDelayCancellationToken;
            private ICancellationToken m_maxDelayCancellationToken;
            private int m_timerAction;
            private int m_sendEmail;

            #endregion

            #region [ Constructors ]

            public EventEmailType(Action<EventEmailParameters, List<Event>> sendEmailCallback)
            {
                m_triggeredEvents = new ConcurrentQueue<Event>();
                m_sendEmailCallback = sendEmailCallback;
                m_synchronizedOperation = new LongSynchronizedOperation(UpdateTimersAndSendEmail);
            }

            #endregion

            #region [ Properties ]

            public EventEmailParameters Parameters
            {
                get => Interlocked.CompareExchange(ref m_parameters, null, null);
                set => Interlocked.Exchange(ref m_parameters, value);
            }

            public int EmailTypeID => Parameters.EmailTypeID;

            #endregion

            #region [ Methods ]

            public void Process(Event evt)
            {
                if (Parameters.TriggersEmail(evt.ID))
                {
                    m_triggeredEvents.Enqueue(evt);
                    m_synchronizedOperation.Run();
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
                int delay = (int)Math.Round(Parameters.MinDelaySpan.TotalMilliseconds);
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
                int delay = (int)Math.Round(Parameters.MaxDelaySpan.TotalMilliseconds);
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
                    m_sendEmailCallback(Parameters, triggeredEvents);
            }

            #endregion
        }

        // Fields
        private EventEmailService m_eventEmailService;
        private List<EventEmailType> m_emailTypes;

        #endregion

        #region [ Constructors ]

        public EventEmailEngine()
        {
            m_eventEmailService = new EventEmailService();
            EmailTypes = new List<EventEmailType>();
        }

        #endregion

        #region [ Properties ]

        private List<EventEmailType> EmailTypes
        {
            get => Interlocked.CompareExchange(ref m_emailTypes, null, null);
            set => Interlocked.Exchange(ref m_emailTypes, value);
        }

        #endregion

        #region [ Methods ]

        public void Process(MeterDataSet meterDataSet)
        {
            m_eventEmailService.ConnectionString = meterDataSet.ConnectionString;
            m_eventEmailService.ConnectionFactory = meterDataSet.CreateDbConnection;
            UpdateEmailTypes();

            using (AdoDataConnection connection = meterDataSet.CreateDbConnection())
            {
                TableOperations<Event> eventTable = new TableOperations<Event>(connection);
                IEnumerable<Event> events = eventTable.QueryRecordsWhere("FileGroupID = {0}", meterDataSet.FileGroup.ID);

                foreach (Event evt in events)
                {
                    foreach (EventEmailType emailType in EmailTypes)
                        emailType.Process(evt);
                }
            }
        }

        public void StartTimer()
        {
            foreach (EventEmailType emailType in EmailTypes)
                emailType.StartTimer();
        }

        public void StopTimer()
        {
            foreach (EventEmailType emailType in EmailTypes)
                emailType.StopTimer();
        }

        private void UpdateEmailTypes()
        {
            List<EventEmailType> emailTypes = new List<EventEmailType>(EmailTypes);
            List<EventEmailParameters> parametersList;

            using (AdoDataConnection connection = m_eventEmailService.ConnectionFactory())
            {
                TableOperations<EventEmailParameters> eventEmailParametersTable = new TableOperations<EventEmailParameters>(connection);
                parametersList = eventEmailParametersTable.QueryRecords().ToList();
                parametersList.ForEach(parameters => parameters.ConnectionFactory = m_eventEmailService.ConnectionFactory);
            }

            List<EventEmailParameters> newParameters = parametersList
                .Where(parameters => !emailTypes.Any(emailType => emailType.EmailTypeID == parameters.EmailTypeID))
                .ToList();

            List<EventEmailType> newEmailTypes = newParameters
                .Select(parameters => new EventEmailType(SendEmail) { Parameters = parameters })
                .ToList();

            foreach (EventEmailType emailType in emailTypes)
            {
                EventEmailParameters parametersUpdate = parametersList
                    .Where(parameters => parameters.EmailTypeID == emailType.EmailTypeID)
                    .FirstOrDefault();

                if ((object)parametersUpdate == null)
                    continue;

                emailType.Parameters = parametersUpdate;
                newEmailTypes.Add(emailType);
            }

            EmailTypes = newEmailTypes;
        }

        private void SendEmail(EventEmailParameters parameters, List<Event> events)
        {
            EmailType emailType;

            using (AdoDataConnection connection = m_eventEmailService.ConnectionFactory())
            {
                TableOperations<EmailType> emailTypeTable = new TableOperations<EmailType>(connection);
                emailType = emailTypeTable.QueryRecordWhere("ID = {0}", parameters.EmailTypeID);

                if ((object)emailType == null)
                    return;

                // Because we are able to rely on event email parameters,
                // we don't really need to check the email category,
                // but it should be validated for the sake of correctness
                TableOperations<EmailCategory> emailCategoryTable = new TableOperations<EmailCategory>(connection);
                EmailCategory emailCategory = emailCategoryTable.QueryRecordWhere("ID = {0}", emailType.EmailCategoryID);

                if (emailCategory.Name != "Event")
                    return;
            }

            while (events.Any())
            {
                string templateData = parameters.GetEventDetail(events[0].ID);
                List<int> eventIDs = m_eventEmailService.GetEventIDs(templateData);
                List<string> recipients = m_eventEmailService.GetRecipients(emailType, eventIDs);
                events.RemoveAll(evt => eventIDs.Contains(evt.ID));

                XDocument htmlDocument = m_eventEmailService.ApplyTemplate(emailType, templateData);
                List<Attachment> attachments = null;

                try
                {
                    attachments = m_eventEmailService.ApplyChartTransform(htmlDocument);
                    m_eventEmailService.SendEmail(recipients, htmlDocument, attachments);
                    m_eventEmailService.LoadSentEmail(recipients, htmlDocument, eventIDs);
                }
                finally
                {
                    attachments?.ForEach(attachment => attachment.Dispose());
                }
            }
        }

        #endregion
    }
}