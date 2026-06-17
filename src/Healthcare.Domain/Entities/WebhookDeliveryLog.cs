using Healthcare.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Domain.Entities
{
    public class WebhookDeliveryLog : BaseEntity
    {
        public Guid WebhookEndpointId { get; set; }

        public Guid PatientId { get; set; }

        public string EventType { get; set; } = string.Empty;

        public string RequestPayload { get; set; } = string.Empty;

        public int? ResponseStatusCode { get; set; }

        public string ResponseBody { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public WebhookEndpoint? WebhookEndpoint { get; set; }
    }
}
