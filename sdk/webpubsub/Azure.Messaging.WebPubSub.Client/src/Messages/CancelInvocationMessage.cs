// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The message representing an invocation cancel request.
    /// </summary>
    public class CancelInvocationMessage : WebPubSubMessage
    {
        /// <summary>
        /// The invocation ID.
        /// </summary>
        public string InvocationId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelInvocationMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        public CancelInvocationMessage(string invocationId)
        {
            InvocationId = invocationId;
        }
    }
}
