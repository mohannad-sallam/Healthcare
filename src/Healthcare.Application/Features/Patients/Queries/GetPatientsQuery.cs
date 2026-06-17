using System;
using System.Collections.Generic;
using System.Text;
using Healthcare.Application.Common.Models;
using Healthcare.Application.Features.Patients.DTOs;
using MediatR;

namespace Healthcare.Application.Features.Patients.Queries
{
    public class GetPatientsQuery : IRequest<PaginationFilteringResults<PatientDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
    }
}
