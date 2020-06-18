// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 1.1.
//
// The APL v2.0:
//
//---------------------------------------------------------------------------
//   Copyright (c) 2007-2020 VMware, Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       https://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//---------------------------------------------------------------------------
//
// The MPL v1.1:
//
//---------------------------------------------------------------------------
//  The contents of this file are subject to the Mozilla Public License
//  Version 1.1 (the "License"); you may not use this file except in
//  compliance with the License. You may obtain a copy of the License
//  at https://www.mozilla.org/MPL/
//
//  Software distributed under the License is distributed on an "AS IS"
//  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See
//  the License for the specific language governing rights and
//  limitations under the License.
//
//  The Original Code is RabbitMQ.
//
//  The Initial Developer of the Original Code is Pivotal Software, Inc.
//  Copyright (c) 2007-2020 VMware, Inc.  All rights reserved.
//---------------------------------------------------------------------------

#pragma warning disable 2002

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using RabbitMQ.Client.Framing.Impl;
using static RabbitMQ.Client.Unit.RabbitMQCtl;

namespace RabbitMQ.Client.Unit
{

    public class IntegrationFixture : IDisposable
    {
        internal IConnectionFactory ConnFactory;
        internal IConnection Conn;
        internal IModel Model;
        internal Encoding encoding = new UTF8Encoding();
        public static TimeSpan RECOVERY_INTERVAL = TimeSpan.FromSeconds(2);

        [SetUp]
        public virtual async ValueTask Init()
        {

            ConnFactory = new ConnectionFactory();
            ConnFactory.ClientProvidedName = GetType().Name;
            Conn = await ConnFactory.CreateConnection();
            Model = await Conn.CreateModel();
        }

        [TearDown]

        public async ValueTask TearDown()
        {
            if (Model.IsOpen)
            {
                await Model.Close();
            }

            if (Conn.IsOpen)
            {
                await Conn.Close();
            }

            ReleaseResources();
        }

        public void Dispose()
        {

        }

        protected virtual void ReleaseResources()
        {
            // no-op
        }

        //
        // Connections
        //

        internal ValueTask<AutorecoveringConnection> CreateAutorecoveringConnection([CallerMemberName] string name = "")
        {
            return CreateAutorecoveringConnection(RECOVERY_INTERVAL, name);
        }

        internal ValueTask<AutorecoveringConnection> CreateAutorecoveringConnection(IList<string> hostnames, [CallerMemberName] string name = "")
        {
            return CreateAutorecoveringConnection(RECOVERY_INTERVAL, hostnames, name);
        }

        internal async ValueTask<AutorecoveringConnection> CreateAutorecoveringConnection(TimeSpan interval, [CallerMemberName] string name = "")
        {
            var cf = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = interval,
                ClientProvidedName = name
            };
            return (AutorecoveringConnection)await cf.CreateConnection();
        }

        internal async ValueTask<AutorecoveringConnection> CreateAutorecoveringConnection(TimeSpan interval, IList<string> hostnames, string name)
        {
            var cf = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                // tests that use this helper will likely list unreachable hosts,
                // make sure we time out quickly on those
                RequestedConnectionTimeout = TimeSpan.FromSeconds(1),
                NetworkRecoveryInterval = interval,
                ClientProvidedName = name
            };
            return (AutorecoveringConnection)await cf.CreateConnection(hostnames);
        }

        internal async ValueTask<AutorecoveringConnection> CreateAutorecoveringConnection(IList<AmqpTcpEndpoint> endpoints, [CallerMemberName] string name = "")
        {
            var cf = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                // tests that use this helper will likely list unreachable hosts,
                // make sure we time out quickly on those
                RequestedConnectionTimeout = TimeSpan.FromSeconds(1),
                NetworkRecoveryInterval = RECOVERY_INTERVAL,
                ClientProvidedName = name
            };
            return (AutorecoveringConnection)await cf.CreateConnection(endpoints);
        }

        internal async ValueTask<AutorecoveringConnection> CreateAutorecoveringConnectionWithTopologyRecoveryDisabled([CallerMemberName] string name = "")
        {
            var cf = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = false,
                NetworkRecoveryInterval = RECOVERY_INTERVAL
            };

            return (AutorecoveringConnection)await cf.CreateConnection(name);
        }

        internal ValueTask<IConnection> CreateNonRecoveringConnection()
        {
            var cf = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = false,
                TopologyRecoveryEnabled = false
            };
            return cf.CreateConnection($"UNIT_CONN:{Guid.NewGuid()}");
        }

        internal ValueTask<IConnection> CreateConnectionWithContinuationTimeout(bool automaticRecoveryEnabled, TimeSpan continuationTimeout)
        {
            var cf = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = automaticRecoveryEnabled,
                ContinuationTimeout = continuationTimeout
            };
            return cf.CreateConnection($"UNIT_CONN:{Guid.NewGuid()}");
        }

        //
        // Channels
        //

        internal async ValueTask WithTemporaryAutorecoveringConnection(Action<AutorecoveringConnection> action)
        {
            var factory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true
            };

            var connection = (AutorecoveringConnection)await factory.CreateConnection($"UNIT_CONN:{Guid.NewGuid()}");
            try
            {
                action(connection);
            }
            finally
            {
                await connection.Abort();
            }
        }

        internal async ValueTask WithTemporaryModel(IConnection connection, Func<IModel, ValueTask> action)
        {
            IModel model = await connection.CreateModel();

            try
            {
                await action(model);
            }
            finally
            {
                model.Abort();
            }
        }

        internal async ValueTask WithTemporaryModel(Func<IModel, ValueTask> action)
        {
            IModel model = await Conn.CreateModel();

            try
            {
                await action(model);
            }
            finally
            {
                model.Abort();
            }
        }

        internal async ValueTask WithClosedModel(Func<IModel, ValueTask> action)
        {
            IModel model = await Conn.CreateModel();
            await model.Close();

            await action(model);
        }

        internal ValueTask<bool> WaitForConfirms(IModel m)
        {
            return m.WaitForConfirms(TimeSpan.FromSeconds(4));
        }

        //
        // Exchanges
        //

        internal string GenerateExchangeName()
        {
            return $"exchange{Guid.NewGuid()}";
        }

        internal byte[] RandomMessageBody()
        {
            return encoding.GetBytes(Guid.NewGuid().ToString());
        }

        internal string DeclareNonDurableExchange(IModel m, string x)
        {
            m.ExchangeDeclare(x, "fanout", false);
            return x;
        }

        internal string DeclareNonDurableExchangeNoWait(IModel m, string x)
        {
            m.ExchangeDeclareNoWait(x, "fanout", false, false, null);
            return x;
        }

        //
        // Queues
        //

        internal string GenerateQueueName()
        {
            return $"queue{Guid.NewGuid()}";
        }

        internal async ValueTask WithTemporaryQueue(Func<IModel, string, ValueTask> action)
        {
            await WithTemporaryQueue(Model, action);
        }

        internal async ValueTask WithTemporaryNonExclusiveQueue(Func<IModel, string, ValueTask> action)
        {
            await WithTemporaryNonExclusiveQueue(Model, action);
        }

        internal async ValueTask WithTemporaryQueue(IModel model, Func<IModel, string, ValueTask> action)
        {
            await WithTemporaryQueue(model, action, GenerateQueueName());
        }

        internal async ValueTask WithTemporaryNonExclusiveQueue(IModel model, Func<IModel, string, ValueTask> action)
        {
            await WithTemporaryNonExclusiveQueue(model, action, GenerateQueueName());
        }

        internal async ValueTask WithTemporaryQueue(Func<IModel, string, ValueTask> action, string q)
        {
            await WithTemporaryQueue(Model, action, q);
        }

        internal async ValueTask WithTemporaryQueue(IModel model, Func<IModel, string, ValueTask> action, string queue)
        {
            try
            {
                await model.QueueDeclare(queue, false, true, false, null);
                await action(model, queue);
            } finally
            {
                await WithTemporaryModel(async x => await x.QueueDelete(queue));
            }
        }

        internal async ValueTask WithTemporaryNonExclusiveQueue(IModel model, Func<IModel, string, ValueTask> action, string queue)
        {
            try
            {
                await model.QueueDeclare(queue, false, false, false, null);
                await action(model, queue);
            } finally
            {
                await WithTemporaryModel(async tm => await tm.QueueDelete(queue));
            }
        }

        internal async ValueTask WithTemporaryQueueNoWait(IModel model, Func<IModel, string, ValueTask> action, string queue)
        {
            try
            {
                await model.QueueDeclareNoWait(queue, false, true, false, null);
                await action(model, queue);
            } finally
            {
                await WithTemporaryModel(async x => await x.QueueDelete(queue));
            }
        }

        internal async ValueTask EnsureNotEmpty(string q)
        {
            await EnsureNotEmpty(q, "msg");
        }

        internal async ValueTask EnsureNotEmpty(string q, string body)
        {
            await WithTemporaryModel(async x => { await x.BasicPublish("", q, null, encoding.GetBytes(body)); await Task.Delay(100);; });
        }

        internal async ValueTask WithNonEmptyQueue(Func<IModel, string, ValueTask> action)
        {
            await WithNonEmptyQueue(action, "msg");
        }

        internal async ValueTask WithNonEmptyQueue(Func<IModel, string, ValueTask> action, string msg)
        {
            await WithTemporaryNonExclusiveQueue(async (m, q) =>
            {
                await EnsureNotEmpty(q, msg);
                await action(m, q);
            });
        }

        internal async ValueTask WithEmptyQueue(Func<IModel, string, ValueTask> action)
        {
            await WithTemporaryNonExclusiveQueue(async (model, queue) =>
            {
                await model.QueuePurge(queue);
                await action(model, queue);
            });
        }

        internal async ValueTask AssertMessageCount(string q, int count)
        {
            await WithTemporaryModel(async (m) => {
                QueueDeclareOk ok = await m.QueueDeclarePassive(q);
                Assert.AreEqual(count, ok.MessageCount);
            });
        }

        internal async ValueTask AssertConsumerCount(string q, int count)
        {
            await WithTemporaryModel(async (m) => {
                QueueDeclareOk ok = await m.QueueDeclarePassive(q);
                Assert.AreEqual(count, ok.ConsumerCount);
            });
        }

        internal async ValueTask AssertConsumerCount(IModel m, string q, int count)
        {
            QueueDeclareOk ok = await m.QueueDeclarePassive(q);
            Assert.AreEqual(count, ok.ConsumerCount);
        }

        //
        // Shutdown
        //

        internal void AssertShutdownError(ShutdownEventArgs args, int code)
        {
            Assert.AreEqual(args.ReplyCode, code);
        }

        internal void AssertPreconditionFailed(ShutdownEventArgs args)
        {
            AssertShutdownError(args, Constants.PreconditionFailed);
        }

        internal bool InitiatedByPeerOrLibrary(ShutdownEventArgs evt)
        {
            return !(evt.Initiator == ShutdownInitiator.Application);
        }

        //
        // Concurrency
        //

        internal void WaitOn(object o)
        {
            lock(o)
            {
                Monitor.Wait(o, TimingFixture.TestTimeout);
            }
        }

        //
        // Flow Control
        //

        internal ValueTask Block()
        {
            return RabbitMQCtl.Block(Conn, encoding);
        }

        internal void Unblock()
        {
            RabbitMQCtl.Unblock();
        }

        internal ValueTask Publish(IConnection conn)
        {
            return RabbitMQCtl.Publish(conn, encoding);
        }

        //
        // Connection Closure
        //

        internal List<ConnectionInfo> ListConnections()
        {
            return RabbitMQCtl.ListConnections();
        }

        internal void CloseConnection(IConnection conn)
        {
            RabbitMQCtl.CloseConnection(conn);
        }

        internal void CloseAllConnections()
        {
            RabbitMQCtl.CloseAllConnections();
        }

        internal void CloseConnection(string pid)
        {
            RabbitMQCtl.CloseConnection(pid);
        }

        internal void RestartRabbitMQ()
        {
            RabbitMQCtl.RestartRabbitMQ();
        }

        internal void StopRabbitMQ()
        {
            RabbitMQCtl.StopRabbitMQ();
        }

        internal void StartRabbitMQ()
        {
            RabbitMQCtl.StartRabbitMQ();
        }

        //
        // Concurrency and Coordination
        //

        internal void Wait(ManualResetEventSlim latch)
        {
            Assert.IsTrue(latch.Wait(TimeSpan.FromSeconds(20)), "waiting on a latch timed out");
        }

        internal void Wait(ManualResetEventSlim latch, TimeSpan timeSpan)
        {
            Assert.IsTrue(latch.Wait(timeSpan), "waiting on a latch timed out");
        }

        //
        // TLS
        //

        public static string CertificatesDirectory()
        {
            return Environment.GetEnvironmentVariable("SSL_CERTS_DIR");
        }
    }

    public class TimingFixture
    {
        public static readonly TimeSpan TimingInterval = TimeSpan.FromMilliseconds(300);
        public static readonly TimeSpan TimingInterval_2X = TimeSpan.FromMilliseconds(600);
        public static readonly TimeSpan SafetyMargin = TimeSpan.FromMilliseconds(150);
        public static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(5);
    }
}
