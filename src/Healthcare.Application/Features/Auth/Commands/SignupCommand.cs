using Healthcare.Application.Features.Auth.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Features.Auth.Commands
{
    public class SignupCommand: IRequest<AuthResponse>
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
