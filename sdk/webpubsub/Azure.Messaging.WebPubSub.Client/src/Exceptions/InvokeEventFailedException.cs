// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// Exception for invoke event failures.
    /// </summary>
    public class InvokeEventFailedException : Exception
    {
        /// <summary>
        /// The invocation ID.
        /// </summary>
        public string InvocationId { get; }

        /// <summary>
        /// The error detail from service when available.
        /// </summary>
        public InvokeResponseError ErrorDetail { get; }

        internal InvokeEventFailedException(string message, string invocationId, InvokeResponseError errorDetail = null)
            : base(message)
        {
            InvocationId = invocationId;
            ErrorDetail = errorDetail;
        }

        internal InvokeEventFailedException(string message, string invocationId, Exception innerException)
            : base(message, innerException)
        {
            InvocationId = invocationId;
            ErrorDetail = null;
        }
    }
}
