//******************************************************************************************************
//  MeterDataScheduler.cs - Gbtc
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
//  08/26/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading;
using GSF.Threading;
using log4net;

namespace openXDA
{
    public class MeterDataScheduler
    {
        #region [ Members ]

        // Nested Types
        private class MeterDataQueue
        {
            #region [ Members ]

            // Fields
            private ConcurrentQueue<string> m_priorityQueue;
            private ConcurrentQueue<string> m_queue;
            private string m_meterKey;
            private int m_isActive;

            #endregion

            #region [ Constructors ]

            public MeterDataQueue(string meterKey)
            {
                m_priorityQueue = new ConcurrentQueue<string>();
                m_queue = new ConcurrentQueue<string>();
                m_meterKey = meterKey;
            }

            #endregion

            #region [ Properties ]

            public string MeterKey
            {
                get
                {
                    return m_meterKey;
                }
            }

            public bool IsEmpty
            {
                get
                {
                    return m_priorityQueue.IsEmpty && m_queue.IsEmpty;
                }
            }

            #endregion

            #region [ Methods ]

            public void PriorityEnqueue(string filePath)
            {
                m_priorityQueue.Enqueue(filePath);
            }

            public void Enqueue(string filePath)
            {
                m_queue.Enqueue(filePath);
            }

            public string Dequeue()
            {
                string filePath;

                if (!m_priorityQueue.TryDequeue(out filePath))
                    m_queue.TryDequeue(out filePath);

                return filePath;
            }

            public bool TryActivate()
            {
                return Interlocked.CompareExchange(ref m_isActive, 1, 0) == 0;
            }

            public void Deactivate()
            {
                Interlocked.Exchange(ref m_isActive, 0);
            }

            #endregion
        }

        // Fields
        private Func<MeterDataProcessor> m_processorFactory;
        private ConcurrentDictionary<string, MeterDataQueue> m_meterDataQueueLookup;
        private ConcurrentQueue<MeterDataQueue> m_meterDataQueues;

        private string m_filePattern;
        private int m_activeThreads;
        private int m_maxThreads;

        #endregion

        #region [ Constructors ]

        public MeterDataScheduler(Func<MeterDataProcessor> processorFactory)
        {
            m_processorFactory = processorFactory;
            m_meterDataQueueLookup = new ConcurrentDictionary<string, MeterDataQueue>();
            m_meterDataQueues = new ConcurrentQueue<MeterDataQueue>();
            MaxThreads = Environment.ProcessorCount;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the regular expression pattern used to
        /// determine the name of the meter from the file path.
        /// </summary>
        public string FilePattern
        {
            get
            {
                return m_filePattern;
            }
            set
            {
                m_filePattern = value;
            }
        }

        /// <summary>
        /// Gets or sets the limit on the number of
        /// active threads processing meter data.
        /// </summary>
        public int MaxThreads
        {
            get
            {
                return Interlocked.CompareExchange(ref m_maxThreads, 0, 0);
            }
            set
            {
                Interlocked.Exchange(ref m_maxThreads, value);

                // If the maximum thread count has increased,
                // there is additional headroom to activate
                // new threads to process active queues
                while (!m_meterDataQueues.IsEmpty && TryActivateThread())
                    ProcessAsync();
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Schedules a file to be processed by a <see cref="MeterDataProcessor"/>
        /// and gives the file higher priority than the files scheduled using the
        /// <see cref="Schedule"/> method.
        /// </summary>
        /// <param name="filePath">The path to the file to be processed.</param>
        public void Prioritize(string filePath)
        {
            string meterKey;
            MeterDataQueue meterDataQueue;

            // Attempt to determine the name
            // of the meter that produced the file
            if (!TryParseFilePath(filePath, out meterKey))
                throw new InvalidOperationException(GetErrorMessage(filePath));

            // Get the queue corresponding to the meter that produced the file
            meterDataQueue = m_meterDataQueueLookup.GetOrAdd(meterKey, key => new MeterDataQueue(key));

            // Put the file into that queue
            meterDataQueue.PriorityEnqueue(filePath);

            // If the queue is not already active,
            // attempt to activate it
            if (meterDataQueue.TryActivate())
            {
                // If the queue was just activated, place the queue into
                // the collection of queues with items to be processed
                m_meterDataQueues.Enqueue(meterDataQueue);

                // Attempt to start a new processing thread
                if (TryActivateThread())
                    ProcessAsync();
            }
        }

        /// <summary>
        /// Schedules a file to be processed by a <see cref="MeterDataProcessor"/>.
        /// </summary>
        /// <param name="filePath">The path to the file to be processed.</param>
        public void Schedule(string filePath)
        {
            string meterKey;
            MeterDataQueue meterDataQueue;

            // Attempt to determine the name
            // of the meter that produced the file
            if (!TryParseFilePath(filePath, out meterKey))
                throw new InvalidOperationException(GetErrorMessage(filePath));

            // Get the queue corresponding to the meter that produced the file
            meterDataQueue = m_meterDataQueueLookup.GetOrAdd(meterKey, key => new MeterDataQueue(key));

            // Put the file into that queue
            meterDataQueue.Enqueue(filePath);

            // If the queue is not already active,
            // attempt to activate it
            if (meterDataQueue.TryActivate())
            {
                // If the queue was just activated, place the queue into
                // the collection of queues with items to be processed
                m_meterDataQueues.Enqueue(meterDataQueue);

                // Attempt to start a new processing thread
                if (TryActivateThread())
                    ProcessAsync();
            }
        }

        private void ProcessAsync()
        {
            LongSynchronizedOperation operation;

            operation = new LongSynchronizedOperation(Process, ex => Log.Error(ex.Message, ex));
            operation.IsBackground = true;
            operation.RunOnceAsync();
        }

        private void Process()
        {
            MeterDataQueue meterDataQueue;

            ThreadContext.Properties["ID"] = Thread.CurrentThread.ManagedThreadId;

            do
            {
                try
                {
                    while (m_activeThreads <= m_maxThreads && m_meterDataQueues.TryDequeue(out meterDataQueue))
                    {
                        // Process the next file in the meter data queue
                        ProcessFile(meterDataQueue.MeterKey, meterDataQueue.Dequeue());

                        if (!meterDataQueue.IsEmpty)
                        {
                            // Queue is still active so we can safely requeue it
                            m_meterDataQueues.Enqueue(meterDataQueue);
                        }
                        else
                        {
                            // Queue is empty, so deactivate it
                            meterDataQueue.Deactivate();

                            // Make sure, after deactivating, that the queue is still empty
                            if (!meterDataQueue.IsEmpty && meterDataQueue.TryActivate())
                                m_meterDataQueues.Enqueue(meterDataQueue);
                        }
                    }
                }
                finally
                {
                    Interlocked.Decrement(ref m_activeThreads);
                }
            }
            while (!m_meterDataQueues.IsEmpty && TryActivateThread());
        }

        private void ProcessFile(string meterKey, string filePath)
        {
            MeterDataProcessor meterDataProcessor;

            try
            {
                // Create a meter data processor to process the next file
                meterDataProcessor = m_processorFactory();
                meterDataProcessor.ProcessFile(meterKey, filePath);
            }
            catch (Exception ex)
            {
                string message = GetErrorMessage(meterKey, filePath, ex);
                Log.Error(message, new Exception(message));
            }
        }

        private bool TryParseFilePath(string fileName, out string meterKey)
        {
            Match match = Regex.Match(fileName, m_filePattern);
            Group meterKeyGroup;

            if (match.Success)
            {
                meterKeyGroup = match.Groups["AssetKey"];

                if ((object)meterKeyGroup != null)
                {
                    meterKey = meterKeyGroup.Value;
                    return true;
                }
            }

            meterKey = null;
            return false;
        }

        private bool TryActivateThread()
        {
            int activeThreads = m_activeThreads;

            while (activeThreads < m_maxThreads)
            {
                // Attempt to increment the number of active threads and return true if successful
                if (Interlocked.CompareExchange(ref m_activeThreads, activeThreads + 1, activeThreads) == activeThreads)
                    return true;

                activeThreads = m_activeThreads;
            }

            // False means the number of active threads
            // exceeds the maximum number of threads
            return false;
        }

        private string GetErrorMessage(string filePath)
        {
            return string.Format("Unable to process file \"{0}\" because no meter " +
                                 "name could be determined based on the regular " +
                                 "expression pattern \"{1}\"", filePath, m_filePattern);
        }

        private string GetErrorMessage(string meterKey, string filePath, Exception ex)
        {
            return string.Format("Unable to process file \"{0}\" from meter \"{1}\" due to exception: {2}", filePath, meterKey, ex.Message);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(MeterDataScheduler));

        #endregion
    }
}
