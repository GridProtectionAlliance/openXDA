//******************************************************************************************************
//  RabbitMQNode.cs - Gbtc
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
//  02/08/2021 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using FaultData.DataAnalysis;
using GSF;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using GSF.IO;
using GSF.Parsing;
using log4net;
using Newtonsoft.Json;
using openXDA.Configuration;
using openXDA.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace openXDA.Nodes.Types.RabbitMQ
{
    public class RabbitMQNode : NodeBase, IDisposable
    {
        #region [ Members ]


        // Nested Types
        private class Settings
        {
            public Settings(Action<object> configure) =>
                configure(this);

            [Category]
            [SettingName(RabbitMQSection.CategoryName)]
            public RabbitMQSection RabbitMQSettings { get; } = new RabbitMQSection();
        }

        private Action<object> Configurator { get; set; }

        private class RabbitMQWebController : ApiController
        {
            private RabbitMQNode Node { get; }

            public RabbitMQWebController(RabbitMQNode node) =>
                Node = node;

            [HttpGet]
            public void Reconfigure() =>
                Node.Reconfigure();

            [HttpPost]
            public void Notify(int fileGroupID, int processingVersion) =>
             Node.SendFileGroup(fileGroupID, processingVersion);

        }

        private bool m_disposed { get; set; }

        private IConnection m_inboundConnection { get; set; }
        private IChannel m_inboundChannel { get; set; }
        private IConnection m_outboundConnection { get; set; }
        private IChannel m_outboundChannel { get; set; }

        #endregion

        public RabbitMQNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            Configurator = GetConfigurator();
            Connect();
        }

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new RabbitMQWebController(this);

        private void Configure(object obj) => Configurator(obj);

        protected override void OnReconfigure(Action<object> configurator)
        {
            Configure(configurator);
            Connect();
        }

        /// <summary>
        /// Create a Connection to the RabbitMQServer
        /// </summary>
        private async Task Connect()
        {
            Settings settings = new Settings(Configure);

            if (!(m_inboundChannel is null))
            {
                m_inboundChannel.Dispose();

            }

            if (!(m_inboundConnection is null))
            {
                m_inboundConnection.Dispose();
            }

            if (!(m_outboundChannel is null))
            {
                m_outboundChannel.Dispose();

            }

            if (!(m_outboundConnection is null))
            {
                m_outboundConnection.Dispose();
            }

            if (!settings.RabbitMQSettings.Enabled)
                return;


            Log.Debug("Creating Connection to RabbitMQ Server.");
            try 
            { 
                var factory = new ConnectionFactory { HostName = settings.RabbitMQSettings.Hostname, Port = settings.RabbitMQSettings.Port, VirtualHost = "/", UserName = "guest" };

                m_inboundConnection = await factory.CreateConnectionAsync().ConfigureAwait(false);
                m_outboundConnection = await factory.CreateConnectionAsync().ConfigureAwait(false);

                m_inboundChannel = await m_inboundConnection.CreateChannelAsync().ConfigureAwait(false);
                m_outboundChannel = await m_outboundConnection.CreateChannelAsync().ConfigureAwait(false);

                await m_outboundChannel.ExchangeDeclareAsync(exchange: settings.RabbitMQSettings.ExchangeName, type: ExchangeType.Direct, durable: true).ConfigureAwait(false);
                await m_inboundChannel.ExchangeDeclareAsync(exchange: settings.RabbitMQSettings.ExchangeName, type: ExchangeType.Direct, durable: true).ConfigureAwait(false);

                await m_inboundChannel.QueueDeclareAsync(queue: "", durable: false, exclusive: true, autoDelete: false, arguments: null);


                QueueDeclareOk queueDeclareResult = await m_inboundChannel.QueueDeclareAsync();

                await m_inboundChannel.QueueBindAsync(queue: queueDeclareResult.QueueName, exchange: settings.RabbitMQSettings.ExchangeName, routingKey: settings.RabbitMQSettings.RoutingKey).ConfigureAwait(false);

                var consumer = new AsyncEventingBasicConsumer(m_inboundChannel);
                consumer.ReceivedAsync += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    string message = Encoding.UTF8.GetString(body);
                    MessageCallback(message);
                    return Task.CompletedTask;
                };

                Task.Run(() => m_inboundChannel.BasicConsumeAsync(queueDeclareResult.QueueName, autoAck: true, consumer: consumer));
            }
            catch (Exception ex)
            {
                Log.Error("Unable to Connect to RabbitMQ Server", ex);
            }

        }

        private void MessageCallback(string message)
        {
            Log.Info("RabbitMQ Message recieved");
            EventTagMessage eventTagMessage = JsonConvert.DeserializeObject<EventTagMessage>(message);

            if (eventTagMessage is null || string.IsNullOrEmpty(eventTagMessage.event_type))
            {
                Log.Warn("Unable to deserialize RabbitMQ message to proper format");
                return;
            }

            using (AdoDataConnection connection = CreateDbConnection())
            {
                EventTag tag = new TableOperations<EventTag>(connection).GetOrAdd(eventTagMessage.event_type);

                // Ensure Event exists
                Event evt = new TableOperations<Event>(connection).QueryRecordWhere("ID = {0}", eventTagMessage.event_id);
                if ( evt is null)
                {
                    Log.Warn($"No event found for event_id {eventTagMessage.event_id}");
                    return;
                }

                EventEventTag entry = new EventEventTag()
                {
                    EventID = eventTagMessage.event_id,
                    EventTagID = tag.ID,
                    TagData = JsonConvert.SerializeObject(eventTagMessage.details)
                };

                new TableOperations<EventEventTag>(connection).AddNewRecord(entry);
            }
        }

        public void Dispose()
        {
            if (m_disposed)
                return;

            try
            {
                m_outboundChannel.Dispose();
                m_inboundChannel.Dispose();
                m_inboundConnection.Dispose();
                m_outboundConnection.Dispose();

            }
            finally
            {
                m_disposed = true;
            }
        }

        public void SendFileGroup(int fileGroupID, int processingVersion)
        {
            Settings settings = new Settings(Configure);
            if (!settings.RabbitMQSettings.Enabled)
                return;

            if (m_outboundChannel is null)
            {
                Log.Error("Cannot send message to RabbitMQ server: No connection established.");
                return;
            }

            // Find All Events
            using (AdoDataConnection connection = CreateDbConnection())
            {
                List<Event> events = new TableOperations<Event>(connection).QueryRecordsWhere("FileGroupID = {0}", fileGroupID).ToList();
                TableOperations<EventType> eventTypeTable = new TableOperations<EventType>(connection);
                foreach (Event evt in events)
                {
                    // Load Event Type
                    EventType eventType = eventTypeTable.QueryRecordWhere("ID = {0}", evt.EventTypeID);
                    
                    VIDataGroup vIData = QueryVIDataGroup(evt.ID, evt.MeterID);

                    if (vIData.DefinedNeutralVoltages == 0)
                    {
                        Log.Warn($"Event {evt.ID} does not have voltage data defined.  Skipping event.");
                        continue;
                    }

                    EventDataMessage eventDataMessage = new EventDataMessage()
                    {
                        event_id = evt.ID,
                        Va = vIData.VA?.DataPoints.Select(item => item.Value).ToArray() ?? Array.Empty<double>(),
                        Vb = vIData.VB?.DataPoints.Select(item => item.Value).ToArray() ?? Array.Empty<double>(),
                        Vc = vIData.VC?.DataPoints.Select(item => item.Value).ToArray() ?? Array.Empty<double>(),
                        sample_rate = evt.SamplesPerCycle,
                        sample_frequency = vIData.VA.SampleRate,
                        event_start_idx = 0,
                        event_type = eventType.Name
                    };

                    byte[] message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventDataMessage));

                    m_outboundChannel.BasicPublishAsync(exchange: settings.RabbitMQSettings.ExchangeName, routingKey: settings.RabbitMQSettings.OutboundRoutingKey, body: message);
                }

                Log.Info($"Sent {events.Count} Events from Filegroup {fileGroupID} to RabbitMQ Server.");

            }
        }

        private VIDataGroup QueryVIDataGroup(int eventID, int meterId)
        {
            using (AdoDataConnection connection = CreateDbConnection())
            {
                List<byte[]> data = ChannelData.DataFromEvent(eventID, CreateDbConnection);
                Meter meter = new TableOperations<Meter>(connection).QueryRecordWhere("ID = {0}", meterId);
                meter.ConnectionFactory = CreateDbConnection;
                return ToVIDataGroup(meter, data);
            }
        }

        private static VIDataGroup ToVIDataGroup(Meter meter, List<byte[]> data)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(meter, data);
            return new VIDataGroup(dataGroup);
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(RabbitMQNode));

      
        #endregion
    }
}
