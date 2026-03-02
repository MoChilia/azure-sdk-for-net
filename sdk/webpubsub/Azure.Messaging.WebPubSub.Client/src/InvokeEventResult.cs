// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.Core;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// Result of invoke event operation.
    /// </summary>
    public class InvokeEventResult
    {
        /// <summary>
        /// The invocation ID.
        /// </summary>
        public string InvocationId { get; }

        /// <summary>
        /// The response payload type.
        /// </summary>
        public WebPubSubDataType? DataType { get; }

        /// <summary>
        /// The response payload.
        /// </summary>
        public BinaryData Data { get; }

        internal InvokeEventResult(string invocationId, WebPubSubDataType? dataType, BinaryData data)
        {
            InvocationId = invocationId;
            DataType = dataType;
            Data = data;
        }
    }
}
