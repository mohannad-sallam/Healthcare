using Healthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Abstractions.Repositories
{
    public interface IWebhookEndpointRepository
    {
        Task<List<WebhookEndpoint>> GetActiveByEventTypeAsync(string eventType, CancellationToken cancellationToken);
    }
}
