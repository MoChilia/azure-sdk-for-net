// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.Core;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The message representing an invocation request.
    /// </summary>
    public class InvokeMessage : WebPubSubMessage
    {
        /// <summary>
        /// The invocation ID.
        /// </summary>
        public string InvocationId { get; }

        /// <summary>
        /// The invocation target.
        /// </summary>
        public string Target { get; }

        /// <summary>
        /// The event name.
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// Type of the data.
        /// </summary>
        public WebPubSubDataType? DataType { get; }

        /// <summary>
        /// The data content.
        /// </summary>
        public BinaryData Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        /// <param name="target">The invocation target.</param>
        /// <param name="eventName">The event name.</param>
        /// <param name="data">The payload data.</param>
        /// <param name="dataType">The payload data type.</param>
        public InvokeMessage(string invocationId, string target, string eventName, BinaryData data = null, WebPubSubDataType? dataType = null)
        {
            InvocationId = invocationId;
            Target = target;
            EventName = eventName;
            Data = data;
            DataType = dataType;
        }
    }
}
