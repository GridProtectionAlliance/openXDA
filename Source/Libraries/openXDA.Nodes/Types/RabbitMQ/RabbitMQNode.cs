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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using FaultData.DataAnalysis;
using GSF.Configuration;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using Newtonsoft.Json;
using openXDA.Configuration;
using openXDA.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

        #endregion

        #region [ Constructors ]

        public RabbitMQNode(Host host, Node definition, NodeType type)
            : base(host, definition, type)
        {
            Action<object> configurator = GetConfigurator();
            Settings settings = new Settings(configurator);
            ConnectAsync(settings).GetAwaiter().GetResult();
        }

        #endregion

        #region [ Properties ]

        private IConnection InboundConnection { get; set; }
        private IChannel InboundChannel { get; set; }
        private IConnection OutboundConnection { get; set; }
        private IChannel OutboundChannel { get; set; }

        private bool IsDisposed { get; set; }

        #endregion

        #region [ Methods ]

        public override IHttpController CreateWebController() =>
            new RabbitMQWebController(this);

        protected override void OnReconfigure(Action<object> configurator)
        {
            Settings settings = new Settings(configurator);
            ConnectAsync(settings).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create a Connection to the RabbitMQServer
        /// </summary>
        private async Task ConnectAsync(Settings settings)
        {
            if (!(InboundChannel is null))
            {
                await InboundChannel.DisposeAsync().ConfigureAwait(false);
            }

            if (!(InboundConnection is null))
            {
                await InboundConnection.DisposeAsync().ConfigureAwait(false);
            }

            if (!(OutboundChannel is null))
            {
                await OutboundChannel.DisposeAsync().ConfigureAwait(false);
            }

            if (!(OutboundConnection is null))
            {
                await OutboundConnection.DisposeAsync().ConfigureAwait(false);
            }

            if (!settings.RabbitMQSettings.Enabled)
                return;

            Log.Debug("Creating Connection to RabbitMQ Server.");

            try
            {
                var factory = new ConnectionFactory { HostName = settings.RabbitMQSettings.Hostname, Port = settings.RabbitMQSettings.Port, VirtualHost = "/", UserName = settings.RabbitMQSettings.UserName, Password = settings.RabbitMQSettings.Password };

                InboundConnection = await factory.CreateConnectionAsync().ConfigureAwait(false);
                OutboundConnection = await factory.CreateConnectionAsync().ConfigureAwait(false);

                InboundChannel = await InboundConnection.CreateChannelAsync().ConfigureAwait(false);
                OutboundChannel = await OutboundConnection.CreateChannelAsync().ConfigureAwait(false);

                await OutboundChannel.ExchangeDeclareAsync(exchange: settings.RabbitMQSettings.ExchangeName, type: ExchangeType.Direct, durable: true).ConfigureAwait(false);
                await InboundChannel.ExchangeDeclareAsync(exchange: settings.RabbitMQSettings.ExchangeName, type: ExchangeType.Direct, durable: true).ConfigureAwait(false);

                await InboundChannel.QueueDeclareAsync(queue: "", durable: false, exclusive: true, autoDelete: false, arguments: null).ConfigureAwait(false);

                QueueDeclareOk queueDeclareResult = await InboundChannel.QueueDeclareAsync().ConfigureAwait(false);

                await InboundChannel.QueueBindAsync(queue: queueDeclareResult.QueueName, exchange: settings.RabbitMQSettings.ExchangeName, routingKey: settings.RabbitMQSettings.RoutingKey).ConfigureAwait(false);

                var consumer = new AsyncEventingBasicConsumer(InboundChannel);
                consumer.ReceivedAsync += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    string message = Encoding.UTF8.GetString(body);
                    MessageCallback(message);
                    return Task.CompletedTask;
                };

                _ = Task.Run(async () =>
                {
                    try { await InboundChannel.BasicConsumeAsync(queueDeclareResult.QueueName, autoAck: true, consumer: consumer).ConfigureAwait(false); }
                    catch (Exception ex) { Log.Error($"RabbitMQ consumer exception: {ex.Message}", ex); }
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to Connect to RabbitMQ Server: {ex.Message}", ex);
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
            if (IsDisposed)
                return;

            try
            {
                OutboundChannel.Dispose();
                InboundChannel.Dispose();
                InboundConnection.Dispose();
                OutboundConnection.Dispose();
            }
            finally
            {
                IsDisposed = true;
            }
        }

        public void SendFileGroup(int fileGroupID, int processingVersion)
        {
            Action<object> configurator = GetConfigurator();
            Settings settings = new Settings(configurator);
            if (!settings.RabbitMQSettings.Enabled)
                return;

            if (OutboundChannel is null)
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

                    OutboundChannel.BasicPublishAsync(exchange: settings.RabbitMQSettings.ExchangeName, routingKey: settings.RabbitMQSettings.OutboundRoutingKey, body: message);
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
