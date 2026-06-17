using System;
using System.Collections.Generic;
using System.Text;
using Healthcare.Application.Features.Patients.DTOs;
using MediatR;

namespace Healthcare.Application.Features.Patients.Queries
{
    public class GetPatientByIdQuery: IRequest<PatientDto?>
    {
        public Guid Id;

        public GetPatientByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
