using Healthcare.Application.Features.Patients.DTOs;
using Healthcare.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Features.Patients.Commands
{
    public class CheckInCommand: IRequest<PatientDto>
    {
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string ReasonForVisit { get; set; }

    }
}
