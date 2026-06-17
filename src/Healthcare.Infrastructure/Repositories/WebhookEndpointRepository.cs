using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Domain.Entities;
using Healthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Infrastructure.Repositories
{
    public class WebhookEndpointRepository : IWebhookEndpointRepository
    {
        private readonly AppDbContext _context;

        public WebhookEndpointRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WebhookEndpoint>> GetActiveByEventTypeAsync(string eventType,CancellationToken cancellationToken)
        {
            var webHook = await _context.WebhookEndpoints
                .Where(endpoint => endpoint.IsActive && endpoint.EventType == eventType)
                .ToListAsync(cancellationToken);

            return webHook;
        }
    }
}
