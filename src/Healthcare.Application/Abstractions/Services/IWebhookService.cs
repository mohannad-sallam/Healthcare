using Healthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Abstractions.Services
{
    public interface IWebhookService
    {
        Task SendPatientCheckedInAsync(Patient patient, CancellationToken cancellationToken);
    }
}
