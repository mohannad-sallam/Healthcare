using System;
using System.Collections.Generic;
using System.Text;
using Healthcare.Application.Abstractions.Services;
using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Domain.Entities;

namespace Healthcare.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }


        public async Task Log(Guid? userId, string action, string tableName, string details, CancellationToken cancellationToken)
        {
            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                TableName = tableName,
                Details = details,
                Timestamp = DateTime.Now
            };
            await _auditLogRepository.AddAuditAsync(auditLog, cancellationToken);

        }
    }
}
