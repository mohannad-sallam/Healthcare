using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Healthcare.Application.Features.Patients.Commands
{
    public class CheckInCommandValidator : AbstractValidator<CheckInCommand>
    {

        public CheckInCommandValidator()
        {
            RuleFor(command => command.FullName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(command => command.PhoneNumber)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(command => command.DateOfBirth)
                .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date of birth must be in the past.");
                
            RuleFor(command => command.ReasonForVisit)
                .NotEmpty()
                .MaximumLength(500);

        }
    }
}
