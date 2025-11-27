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
        }

        private bool m_disposed { get; set; }

        private IConnection m_connection { get; set; }
        private IChannel m_channel { get; set; }
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

            if (!(m_channel is null))
            {
                m_channel.Dispose();

            }

            if (!(m_connection is null))
            {
                m_connection.Dispose();
            }

            if (!settings.RabbitMQSettings.Enabled)
                return;


            Log.Debug("Creating Connection to RabbitMQ Server.");
            try 
            { 
                var factory = new ConnectionFactory { HostName = settings.RabbitMQSettings.Hostname, Port = settings.RabbitMQSettings.Port, VirtualHost = "/", UserName = "guest" };

                m_connection = await factory.CreateConnectionAsync().ConfigureAwait(false);
                m_channel = await m_connection.CreateChannelAsync().ConfigureAwait(false);
                await m_channel.ExchangeDeclareAsync(exchange: settings.RabbitMQSettings.ExchangeName, type: ExchangeType.Direct, durable: true).ConfigureAwait(false);

                await m_channel.QueueDeclareAsync(queue: "hello", durable: true, exclusive: true, autoDelete: false, arguments: null);


                QueueDeclareOk queueDeclareResult = await m_channel.QueueDeclareAsync();
                await m_channel.QueueBindAsync(queue: queueDeclareResult.QueueName, exchange: settings.RabbitMQSettings.ExchangeName, routingKey: settings.RabbitMQSettings.RoutingKey).ConfigureAwait(false);

                var consumer = new AsyncEventingBasicConsumer(m_channel);
                consumer.ReceivedAsync += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    string message = Encoding.UTF8.GetString(body);
                    MessageCallback(message);
                    return Task.CompletedTask;
                };

                Task.Run(() => m_channel.BasicConsumeAsync(queueDeclareResult.QueueName, autoAck: true, consumer: consumer));
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
                m_channel.Dispose();
                m_connection.Dispose();

            }
            finally
            {
                m_disposed = true;
            }
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(RabbitMQNode));

        #endregion
    }
}
