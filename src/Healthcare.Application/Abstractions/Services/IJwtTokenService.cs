using Healthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Abstractions.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
