using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Features.Auth.Commands
{
    public class SignupCommandValidator: AbstractValidator<SignupCommand>
    {
        public SignupCommandValidator()
        {
            RuleFor(command => command.FullName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(command => command.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(200);

            RuleFor(command => command.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}
