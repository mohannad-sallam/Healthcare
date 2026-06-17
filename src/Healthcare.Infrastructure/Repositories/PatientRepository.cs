using Healthcare.Application.Abstractions.Repositories;
using Healthcare.Domain.Entities;
using Healthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Healthcare.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Infrastructure.Repositories
{
    public class PatientRepository:IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context= context;
        }

        public async Task<Patient> CreatePatient(Patient patient, CancellationToken cancellationToken)
        {
            await _context.Patient.AddAsync(patient,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return patient;
        }

        public async Task<PaginationFilteringResults<Patient>> GetAllPatientsPaged(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken)
        {
            var query =  _context.Patient
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query=query.Where(q=> q.FullName.Contains(search) || q.PhoneNumber.Contains(search) ||q.ReasonForVisit.Contains(search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var patients = await query
                .OrderByDescending(q=>q.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginationFilteringResults<Patient>
            {
                Items = patients,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<Patient?> GetPatientById(Guid Id,CancellationToken cancellationToken)
        {
            var patient=await _context.Patient.FirstOrDefaultAsync(x => x.Id==Id);

            return patient;
        }
    }
}
