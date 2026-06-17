using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Application.Abstractions.Services;
using Healthcare.Application.Features.Patients.DTOs;
using MediatR;
using Healthcare.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Features.Patients.Queries
{
    public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, PatientDto?>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAuditService _auditService;
        private readonly ICurrentUserService _currentUserService;

        public GetPatientByIdQueryHandler(IPatientRepository patientRepository, IAuditService auditService, ICurrentUserService currentUserService)
        {
            _patientRepository = patientRepository;
            _auditService = auditService;
            _currentUserService = currentUserService;
        }

        public async Task<PatientDto?> Handle(GetPatientByIdQuery request,CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            if (currentUserId is null)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            var patient = await _patientRepository.GetPatientById(request.Id,cancellationToken);

            if (patient is null)
            {
                throw new NotFoundException("Patient not found");
            }

            await _auditService.Log(_currentUserService.UserId,"PatientViewed","Patients",$"Patient with id {patient.Id} was viewed.",cancellationToken);

            var patientDto= new PatientDto
            {
                Id=patient.Id,
                FullName = patient.FullName,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth,
                ReasonForVisit = patient.ReasonForVisit,
                CreatedAt = patient.CreatedAt
            };

            return patientDto;
        }
    }
}
