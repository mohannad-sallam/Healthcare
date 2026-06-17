using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Domain.Entities;
using Healthcare.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Infrastructure.Repositories
{
    public class WebhookDeliveryLogRepository : IWebhookDeliveryLogRepository
    {
        private readonly AppDbContext _context;

        public WebhookDeliveryLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WebhookDeliveryLog log, CancellationToken cancellationToken)
        {
            await _context.WebhookDeliveryLogs.AddAsync(log, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
