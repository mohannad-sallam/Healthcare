using Healthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Abstractions.Repositories
{
    public interface IAuditLogRepository
    {
        Task AddAuditAsync(AuditLog auditLog, CancellationToken cancellationToken);
    }
}
