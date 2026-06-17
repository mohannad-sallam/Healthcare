using System;
using System.Collections.Generic;
using System.Text;
using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Domain.Entities;
using Healthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            return user;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _context.User.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user!=null) return true;
            else return false;
        }
    }
}
