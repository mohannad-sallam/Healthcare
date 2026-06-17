using System;
using System.Collections.Generic;
using System.Text;
using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Infrastructure.Persistence;
using Healthcare.Domain.Entities;


namespace Healthcare.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAuditAsync(AuditLog auditLog, CancellationToken cancellationToken)
        {
            await _context.AuditLog.AddAsync(auditLog, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
