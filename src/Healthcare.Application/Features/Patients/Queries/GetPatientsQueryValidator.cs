using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using MediatR;

namespace Healthcare.Application.Features.Patients.Queries
{
    public class GetPatientsQueryValidator:AbstractValidator<GetPatientsQuery>
    {

        public GetPatientsQueryValidator()
        {
            RuleFor(p => p.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be a positive integer.");

            RuleFor(p=>p.PageSize)
                .InclusiveBetween(1, 100)   
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(p => p.Search)
                .MaximumLength(100)
                .WithMessage("Search term cannot exceed 100 characters.");
        }
    }
}
