using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Application.Abstractions.Services;
using Healthcare.Application.Common.Exceptions;
using Healthcare.Application.Features.Auth.DTOs;
using Healthcare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Features.Auth.Commands
{
    public class SignupCommandHandler : IRequestHandler<SignupCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAuditService _auditService;
        private readonly PasswordHasher<User> _passwordHasher;

        public SignupCommandHandler(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IAuditService auditService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _auditService = auditService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<AuthResponse> Handle(
            SignupCommand request,
            CancellationToken cancellationToken)
        {
            var emailExists = await _userRepository.EmailExistsAsync(
                request.Email,
                cancellationToken);

            if (emailExists)
            {
                throw new BadRequestException("Email is already registered.");
            }

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = string.Empty
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.AddAsync(user, cancellationToken);

            await _auditService.Log(user.Id, "UserSignedUp", "Users", $"User with email {user.Email} signed up.", cancellationToken);

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email
            };
        }
    }
}
