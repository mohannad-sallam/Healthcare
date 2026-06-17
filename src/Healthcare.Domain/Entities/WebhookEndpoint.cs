using Healthcare.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Domain.Entities
{
    public class WebhookEndpoint : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string EventType { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
