// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using Azure.Messaging.WebPubSub.Client.Tests.Utils;
using Azure.Messaging.WebPubSub.Clients;
using Moq;
using NUnit.Framework;

namespace Azure.Messaging.WebPubSub.Client.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class WebPubSubClientInvokeEventTests
    {
        private Mock<IWebSocketClient> _webSocketClientMoc;
        private Mock<IWebSocketClientFactory> _factoryMoc;

        [SetUp]
        public void WebPubSubClientInvokeEventTestsSetup()
        {
            _webSocketClientMoc = new Mock<IWebSocketClient>();
            _webSocketClientMoc.SetReturnsDefault(Task.CompletedTask);
            _webSocketClientMoc.Setup(c => c.ReceiveOneFrameAsync(It.IsAny<CancellationToken>())).Returns<CancellationToken>(async token =>
            {
                await Task.Delay(int.MaxValue).AwaitWithCancellation(token);
                return new WebSocketReadResult(default, false);
            });
            _factoryMoc = new Mock<IWebSocketClientFactory>();
            _factoryMoc.Setup(f => f.CreateWebSocketClient(It.IsAny<Uri>(), It.IsAny<string>())).Returns(_webSocketClientMoc.Object);
        }

        [Test]
        public async Task InvokeEventIntegrationTest_Success()
        {
            var wsPair = new TestWebSocketClientPair(_webSocketClientMoc);
            var client = new WebPubSubClient(new Uri("wss://test.com"));
            client.WebSocketClientFactory = _factoryMoc.Object;

            await client.StartAsync();

            var invokeTask = client.InvokeEventAsync("echo", BinaryData.FromString("ping"), WebPubSubDataType.Text, "invoke-id");

            var invokePayload = await wsPair.Output().OrTimeout();
            using (var document = JsonDocument.Parse(Encoding.UTF8.GetString(invokePayload.ToArray())))
            {
                var root = document.RootElement;
                Assert.AreEqual("invoke", root.GetProperty("type").GetString());
                Assert.AreEqual("invoke-id", root.GetProperty("invocationId").GetString());
                Assert.AreEqual("event", root.GetProperty("target").GetString());
                Assert.AreEqual("echo", root.GetProperty("event").GetString());
                Assert.AreEqual("Text", root.GetProperty("dataType").GetString());
                Assert.AreEqual("ping", root.GetProperty("data").GetString());
            }

            wsPair.Input(TestUtils.GetInvokeResponsePayload("invoke-id", true, dataType: "text", data: "pong"), false);
            var result = await invokeTask.OrTimeout();

            Assert.AreEqual("invoke-id", result.InvocationId);
            Assert.AreEqual(WebPubSubDataType.Text, result.DataType);
            Assert.AreEqual("pong", result.Data.ToString());
        }

        [Test]
        public async Task InvokeEventIntegrationTest_ServiceError()
        {
            var wsPair = new TestWebSocketClientPair(_webSocketClientMoc);
            var client = new WebPubSubClient(new Uri("wss://test.com"));
            client.WebSocketClientFactory = _factoryMoc.Object;

            await client.StartAsync();

            var invokeTask = client.InvokeEventAsync("echo", BinaryData.FromString("ping"), WebPubSubDataType.Text, "invoke-error");
            await wsPair.Output().OrTimeout();

            wsPair.Input(TestUtils.GetInvokeResponsePayload("invoke-error", false, "BadRequest", "oops"), false);

            var ex = Assert.ThrowsAsync<InvokeEventFailedException>(() => invokeTask);
            Assert.AreEqual("invoke-error", ex.InvocationId);
            Assert.NotNull(ex.ErrorDetail);
            Assert.AreEqual("BadRequest", ex.ErrorDetail.Name);
            Assert.AreEqual("oops", ex.ErrorDetail.Message);
        }

        [Test]
        public async Task InvokeEventIntegrationTest_CancelSendCancelInvocation()
        {
            var wsPair = new TestWebSocketClientPair(_webSocketClientMoc);
            var client = new WebPubSubClient(new Uri("wss://test.com"));
            client.WebSocketClientFactory = _factoryMoc.Object;

            await client.StartAsync();

            var cts = new CancellationTokenSource();
            var invokeTask = client.InvokeEventAsync("echo", BinaryData.FromString("ping"), WebPubSubDataType.Text, "invoke-cancel", cts.Token);
            await wsPair.Output().OrTimeout();

            cts.Cancel();
            var cancelPayload = await wsPair.Output().OrTimeout();
            using (var document = JsonDocument.Parse(Encoding.UTF8.GetString(cancelPayload.ToArray())))
            {
                var root = document.RootElement;
                Assert.AreEqual("cancelInvocation", root.GetProperty("type").GetString());
                Assert.AreEqual("invoke-cancel", root.GetProperty("invocationId").GetString());
            }

            var ex = Assert.ThrowsAsync<InvokeEventFailedException>(() => invokeTask);
            Assert.AreEqual("invoke-cancel", ex.InvocationId);
        }
    }
}
