using Healthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

        Task AddAsync(User user, CancellationToken cancellationToken);

        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
    }
}
