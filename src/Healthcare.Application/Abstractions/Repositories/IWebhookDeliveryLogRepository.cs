using Healthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Abstractions.Repositories
{
    public interface IWebhookDeliveryLogRepository
    {
        Task AddAsync(WebhookDeliveryLog log, CancellationToken cancellationToken);
    }
}
