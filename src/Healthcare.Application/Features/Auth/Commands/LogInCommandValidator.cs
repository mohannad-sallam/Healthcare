using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;


namespace Healthcare.Application.Features.Auth.Commands
{
    public class LogInCommandValidator: AbstractValidator<LogInCommand>
    {
        public LogInCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}
