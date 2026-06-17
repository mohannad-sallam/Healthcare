using Healthcare.Application.Features.Auth.DTOs;
using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Application.Abstractions.Services;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Healthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Healthcare.Application.Features.Auth.Commands
{
    public class LogInCommandHandler: IRequestHandler<LogInCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAuditService _auditService;
        private readonly PasswordHasher<User> _passwordHasher;

        public LogInCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IAuditService auditService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _auditService = auditService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<AuthResponse> Handle(LogInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var token = _jwtTokenService.GenerateToken(user);

            await _auditService.Log(user.Id, "Successful Login", "User", $"Email: {request.Email}", cancellationToken);

            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email
            };
        }
    }
}
