using System;
using System.Collections.Generic;
using System.Text;
using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Application.Abstractions.Services;
using Healthcare.Application.Features.Patients.DTOs;
using Healthcare.Domain.Entities;
using MediatR;


namespace Healthcare.Application.Features.Patients.Commands
{
    public class CheckInCommandHandler : IRequestHandler<CheckInCommand, PatientDto>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAuditService _auditService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWebhookService _webhookService;

        public CheckInCommandHandler(IPatientRepository patientRepository, IAuditService auditService, ICurrentUserService currentUserService, IWebhookService webhookService)
        {
            _patientRepository = patientRepository;
            _auditService = auditService;
            _currentUserService = currentUserService;
            _webhookService = webhookService;
        }

        public async Task<PatientDto> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {

            var currentUserId = _currentUserService.UserId;

            if (currentUserId is null)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth,
                ReasonForVisit = request.ReasonForVisit,
                CreatedByUserId = currentUserId.Value
            };

            await _patientRepository.CreatePatient(patient, cancellationToken);

            await _auditService.Log(currentUserId, "Patient checked in", "Patient", $"Patient {patient.FullName} Checked-In", cancellationToken);

            await _webhookService.SendPatientCheckedInAsync(patient, cancellationToken);

            var patientDto = new PatientDto
            {
                Id=patient.Id,
                FullName = patient.FullName,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth,
                ReasonForVisit = patient.ReasonForVisit,
            };

            return patientDto;
        }

    }
}
