using Healthcare.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Domain.Entities
{
    public class User: BaseEntity
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    }
}
