using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Features.Patients.DTOs
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public string ReasonForVisit { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
