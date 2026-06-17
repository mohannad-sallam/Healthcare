using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Application.Abstractions.Services;
using MediatR;
using Healthcare.Application.Features.Patients.DTOs;
using Healthcare.Application.Common.Exceptions;
using Healthcare.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Features.Patients.Queries
{
    public class GetPatientsQueryHandler:IRequestHandler<GetPatientsQuery, PaginationFilteringResults<PatientDto>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAuditService _auditService;
        private readonly ICurrentUserService _currentUserService;

        public GetPatientsQueryHandler(IPatientRepository patientRepository, IAuditService auditService, ICurrentUserService currentUserService)
        {
            _patientRepository = patientRepository;
            _auditService = auditService;
            _currentUserService = currentUserService;
        }

        public async Task<PaginationFilteringResults<PatientDto>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            if (currentUserId is null)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            var patients = await _patientRepository.GetAllPatientsPaged(request.PageNumber, request.PageSize, request.Search, cancellationToken);

            if (patients == null) {
                throw new NotFoundException("No patients found");
            }

            await _auditService.Log(currentUserId, "Get Patients", "Patient", $"Retrieved all patients.", cancellationToken);

            var PagedResults = new PaginationFilteringResults<PatientDto>
            {
                Items = patients.Items.Select(p => new PatientDto
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    PhoneNumber = p.PhoneNumber,
                    DateOfBirth = p.DateOfBirth,
                    ReasonForVisit = p.ReasonForVisit,
                    CreatedAt = p.CreatedAt
                }).ToList(),
                PageNumber = patients.PageNumber,
                PageSize = patients.PageSize,
                TotalCount = patients.TotalCount
            };

            return PagedResults;
        }
    }
}
