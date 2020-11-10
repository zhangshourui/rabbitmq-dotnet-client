// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 2.0.
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
// The MPL v2.0:
//
//---------------------------------------------------------------------------
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
//
//  Copyright (c) 2007-2020 VMware, Inc.  All rights reserved.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client.Impl;

namespace RabbitMQ.Client.Framing
{
    /// <summary>Autogenerated type. AMQP specification content header properties for content class "basic"</summary>
    internal sealed class BasicProperties : RabbitMQ.Client.Impl.BasicProperties
    {
        private string _contentType;
        private string _contentEncoding;
        private IDictionary<string, object> _headers;
        private byte _deliveryMode;
        private byte _priority;
        private string _correlationId;
        private string _replyTo;
        private string _expiration;
        private string _messageId;
        private AmqpTimestamp _timestamp;
        private string _type;
        private string _userId;
        private string _appId;
        private string _clusterId;

        public override string ContentType
        {
            get => _contentType;
            set => _contentType = value;
        }

        public override string ContentEncoding
        {
            get => _contentEncoding;
            set => _contentEncoding = value;
        }

        public override IDictionary<string, object> Headers
        {
            get => _headers;
            set => _headers = value;
        }

        public override byte DeliveryMode
        {
            get => _deliveryMode;
            set => _deliveryMode = value;
        }

        public override byte Priority
        {
            get => _priority;
            set => _priority = value;
        }

        public override string CorrelationId
        {
            get => _correlationId;
            set => _correlationId = value;
        }

        public override string ReplyTo
        {
            get => _replyTo;
            set => _replyTo = value;
        }

        public override string Expiration
        {
            get => _expiration;
            set => _expiration = value;
        }

        public override string MessageId
        {
            get => _messageId;
            set => _messageId = value;
        }

        public override AmqpTimestamp Timestamp
        {
            get => _timestamp;
            set => _timestamp = value;
        }

        public override string Type
        {
            get => _type;
            set => _type = value;
        }

        public override string UserId
        {
            get => _userId;
            set => _userId = value;
        }

        public override string AppId
        {
            get => _appId;
            set => _appId = value;
        }

        public override string ClusterId
        {
            get => _clusterId;
            set => _clusterId = value;
        }

        public override void ClearContentType() => _contentType = default;

        public override void ClearContentEncoding() => _contentEncoding = default;

        public override void ClearHeaders() => _headers = default;

        public override void ClearDeliveryMode() => _deliveryMode = default;

        public override void ClearPriority() => _priority = default;

        public override void ClearCorrelationId() => _correlationId = default;

        public override void ClearReplyTo() => _replyTo = default;

        public override void ClearExpiration() => _expiration = default;

        public override void ClearMessageId() => _messageId = default;

        public override void ClearTimestamp() => _timestamp = default;

        public override void ClearType() => _type = default;

        public override void ClearUserId() => _userId = default;

        public override void ClearAppId() => _appId = default;

        public override void ClearClusterId() => _clusterId = default;

        public override bool IsContentTypePresent() => _contentType != default;

        public override bool IsContentEncodingPresent() => _contentEncoding != default;

        public override bool IsHeadersPresent() => _headers != default;

        public override bool IsDeliveryModePresent() => _deliveryMode != default;

        public override bool IsPriorityPresent() => _priority != default;

        public override bool IsCorrelationIdPresent() => _correlationId != default;

        public override bool IsReplyToPresent() => _replyTo != default;

        public override bool IsExpirationPresent() => _expiration != default;

        public override bool IsMessageIdPresent() => _messageId != default;

        public override bool IsTimestampPresent() => _timestamp != default;

        public override bool IsTypePresent() => _type != default;

        public override bool IsUserIdPresent() => _userId != default;

        public override bool IsAppIdPresent() => _appId != default;

        public override bool IsClusterIdPresent() => _clusterId != default;

        public BasicProperties()
        {
        }

        public BasicProperties(ReadOnlySpan<byte> span)
        {
            int offset = WireFormatting.ReadBits(span,
                out bool contentType_present,
                out bool contentEncoding_present,
                out bool headers_present,
                out bool deliveryMode_present,
                out bool priority_present,
                out bool correlationId_present,
                out bool replyTo_present,
                out bool expiration_present,
                out bool messageId_present,
                out bool timestamp_present,
                out bool type_present,
                out bool userId_present,
                out bool appId_present,
                out bool clusterId_present);
            if (contentType_present) { offset += WireFormatting.ReadShortstr(span.Slice(offset), out _contentType); }
            if (contentEncoding_present) { offset += WireFormatting.ReadShortstr(span.Slice(offset), out _contentEncoding); }
            if (headers_present) { offset += WireFormatting.ReadDictionary(span.Slice(offset), out var tmpDirectory); _headers = tmpDirectory; }
            if (deliveryMode_present) { _deliveryMode = span[offset++]; }
            if (priority_present) { _priority = span[offset++]; }
            if (correlationId_present) { offset += WireFormatting.ReadShortstr(span.Slice(offset), out _correlationId); }
            if (replyTo_present) { offset += WireFormatting.ReadShortstr(span.Slice(offset), out _replyTo); }
            if (expiration_present) { offset += WireFormatting.ReadShortstr(span.Slice(offset), out _expiration); }
            if (messageId_present) { offset += WireFormatting.ReadShortstr(span.Slice(offset), out _messageId); }
            if (timestamp_present) { offset += WireFormatting.ReadTimestamp(span.Slice(offset), out _timestamp); }
            if (type_present) { offset += WireFormatting.ReadShortstr(span.Slice(offset), out _type); }
            if (userId_present) { offset += WireFormatting.ReadShortstr(span.Slice(offset), out _userId); }
            if (appId_present) { offset += WireFormatting.ReadShortstr(span.Slice(offset), out _appId); }
            if (clusterId_present) { WireFormatting.ReadShortstr(span.Slice(offset), out _clusterId); }
        }

        public override ushort ProtocolClassId => 60;
        public override string ProtocolClassName => "basic";

        internal override int WritePropertiesTo(Span<byte> span)
        {
            int offset = WireFormatting.WriteBits(span,
                IsContentTypePresent(), IsContentEncodingPresent(), IsHeadersPresent(), IsDeliveryModePresent(), IsPriorityPresent(),
                IsCorrelationIdPresent(), IsReplyToPresent(), IsExpirationPresent(), IsMessageIdPresent(), IsTimestampPresent(),
                IsTypePresent(), IsUserIdPresent(), IsAppIdPresent(), IsClusterIdPresent());
            if (IsContentTypePresent()) { offset += WireFormatting.WriteShortstr(span.Slice(offset), _contentType); }
            if (IsContentEncodingPresent()) { offset += WireFormatting.WriteShortstr(span.Slice(offset), _contentEncoding); }
            if (IsHeadersPresent()) { offset += WireFormatting.WriteTable(span.Slice(offset), _headers); }
            if (IsDeliveryModePresent()) { span[offset++] = _deliveryMode; }
            if (IsPriorityPresent()) { span[offset++] = _priority; }
            if (IsCorrelationIdPresent()) { offset += WireFormatting.WriteShortstr(span.Slice(offset), _correlationId); }
            if (IsReplyToPresent()) { offset += WireFormatting.WriteShortstr(span.Slice(offset), _replyTo); }
            if (IsExpirationPresent()) { offset += WireFormatting.WriteShortstr(span.Slice(offset), _expiration); }
            if (IsMessageIdPresent()) { offset += WireFormatting.WriteShortstr(span.Slice(offset), _messageId); }
            if (IsTimestampPresent()) { offset += WireFormatting.WriteTimestamp(span.Slice(offset), _timestamp); }
            if (IsTypePresent()) { offset += WireFormatting.WriteShortstr(span.Slice(offset), _type); }
            if (IsUserIdPresent()) { offset += WireFormatting.WriteShortstr(span.Slice(offset), _userId); }
            if (IsAppIdPresent()) { offset += WireFormatting.WriteShortstr(span.Slice(offset), _appId); }
            if (IsClusterIdPresent()) { offset += WireFormatting.WriteShortstr(span.Slice(offset), _clusterId); }
            return offset;
        }

        public override int GetRequiredPayloadBufferSize()
        {
            int bufferSize = 2; // number of presence fields (14) in 2 bytes blocks
            if (IsContentTypePresent()) { bufferSize += 1 + Encoding.UTF8.GetByteCount(_contentType); } // _contentType in bytes
            if (IsContentEncodingPresent()) { bufferSize += 1 + Encoding.UTF8.GetByteCount(_contentEncoding); } // _contentEncoding in bytes
            if (IsHeadersPresent()) { bufferSize += WireFormatting.GetTableByteCount(_headers); } // _headers in bytes
            if (IsDeliveryModePresent()) { bufferSize++; } // _deliveryMode in bytes
            if (IsPriorityPresent()) { bufferSize++; } // _priority in bytes
            if (IsCorrelationIdPresent()) { bufferSize += 1 + Encoding.UTF8.GetByteCount(_correlationId); } // _correlationId in bytes
            if (IsReplyToPresent()) { bufferSize += 1 + Encoding.UTF8.GetByteCount(_replyTo); } // _replyTo in bytes
            if (IsExpirationPresent()) { bufferSize += 1 + Encoding.UTF8.GetByteCount(_expiration); } // _expiration in bytes
            if (IsMessageIdPresent()) { bufferSize += 1 + Encoding.UTF8.GetByteCount(_messageId); } // _messageId in bytes
            if (IsTimestampPresent()) { bufferSize += 8; } // _timestamp in bytes
            if (IsTypePresent()) { bufferSize += 1 + Encoding.UTF8.GetByteCount(_type); } // _type in bytes
            if (IsUserIdPresent()) { bufferSize += 1 + Encoding.UTF8.GetByteCount(_userId); } // _userId in bytes
            if (IsAppIdPresent()) { bufferSize += 1 + Encoding.UTF8.GetByteCount(_appId); } // _appId in bytes
            if (IsClusterIdPresent()) { bufferSize += 1 + Encoding.UTF8.GetByteCount(_clusterId); } // _clusterId in bytes
            return bufferSize;
        }
    }
}
