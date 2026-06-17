using Healthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Healthcare.Application.Common.Models;
namespace Healthcare.Application.Abstractions.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> CreatePatient(Patient patient,CancellationToken cancellationToken);
        Task<Patient?> GetPatientById(Guid Id, CancellationToken cancellationToken);
        Task<PaginationFilteringResults<Patient>> GetAllPatientsPaged(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken);
    }
}
