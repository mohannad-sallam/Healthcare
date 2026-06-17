using Healthcare.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Domain.Entities
{
    public class AuditLog: BaseEntity
    {
        public Guid? UserId { get; set; }

        public string Action { get; set; } = string.Empty;

        public string TableName { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public User? User { get; set; }
    }
}
