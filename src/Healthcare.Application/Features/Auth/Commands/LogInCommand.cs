using MediatR;
using Healthcare.Application.Features.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Features.Auth.Commands
{
    public class LogInCommand: IRequest<AuthResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }= string.Empty;
    }
}
