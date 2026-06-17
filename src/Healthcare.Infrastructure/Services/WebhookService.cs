using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Application.Abstractions.Services;
using Healthcare.Domain.Entities;
using System.Net.Http.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Healthcare.Infrastructure.Services
{
    public class WebhookService : IWebhookService
    {
        private const string PatientCheckedInEvent = "patient.checked_in";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebhookEndpointRepository _webhookEndpointRepository;
        private readonly IWebhookDeliveryLogRepository _webhookDeliveryLogRepository;

        public WebhookService(IHttpClientFactory httpClientFactory, IWebhookEndpointRepository webhookEndpointRepository, IWebhookDeliveryLogRepository webhookDeliveryLogRepository)
        {
            _httpClientFactory = httpClientFactory;
            _webhookEndpointRepository = webhookEndpointRepository;
            _webhookDeliveryLogRepository = webhookDeliveryLogRepository;
        }

        public async Task SendPatientCheckedInAsync(Patient patient, CancellationToken cancellationToken)
        {
            var endpoints = await _webhookEndpointRepository.GetActiveByEventTypeAsync(PatientCheckedInEvent, cancellationToken);

            if (endpoints.Count == 0)
            {
                return;
            }

            var payload = new
            {
                EventType = PatientCheckedInEvent,
                PatientId = patient.Id,
                patient.FullName,
                patient.PhoneNumber,
                patient.DateOfBirth,
                patient.ReasonForVisit,
                patient.CreatedAt
            };

            var requestPayload = JsonSerializer.Serialize(payload);

            foreach (var endpoint in endpoints)
            {
                await SendToEndpointAsync(endpoint, patient.Id, requestPayload, payload, cancellationToken);
            }
        }

        private async Task SendToEndpointAsync(WebhookEndpoint endpoint, Guid patientId, string requestPayload, object payload, CancellationToken cancellationToken)
        {
            var log = new WebhookDeliveryLog
            {
                WebhookEndpointId = endpoint.Id,
                PatientId = patientId,
                EventType = PatientCheckedInEvent,
                RequestPayload = requestPayload
            };

            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.PostAsJsonAsync(
                    endpoint.Url,
                    payload,
                    cancellationToken);

                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                log.ResponseStatusCode = (int)response.StatusCode;
                log.ResponseBody = responseBody;
                log.IsSuccess = response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                log.IsSuccess = false;
                log.ErrorMessage = exception.Message;
            }

            await _webhookDeliveryLogRepository.AddAsync(log, cancellationToken);
        }
    }
}
