// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.Core;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The message representing an invocation response.
    /// </summary>
    public class InvokeResponseMessage : WebPubSubMessage
    {
        /// <summary>
        /// The invocation ID.
        /// </summary>
        public string InvocationId { get; }

        /// <summary>
        /// Whether the invocation succeeds.
        /// </summary>
        public bool? Success { get; }

        /// <summary>
        /// The data type.
        /// </summary>
        public WebPubSubDataType? DataType { get; }

        /// <summary>
        /// The payload data.
        /// </summary>
        public BinaryData Data { get; }

        /// <summary>
        /// The error detail when invocation fails.
        /// </summary>
        public InvokeResponseError Error { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeResponseMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        /// <param name="success">Whether the invocation succeeds.</param>
        /// <param name="dataType">The payload data type.</param>
        /// <param name="data">The payload data.</param>
        /// <param name="error">The invocation error detail.</param>
        public InvokeResponseMessage(string invocationId, bool? success, WebPubSubDataType? dataType = null, BinaryData data = null, InvokeResponseError error = null)
        {
            InvocationId = invocationId;
            Success = success;
            DataType = dataType;
            Data = data;
            Error = error;
        }
    }
}
