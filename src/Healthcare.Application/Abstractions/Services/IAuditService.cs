using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Abstractions.Services
{
    public interface IAuditService
    {
        Task Log(Guid? userId, string action, string tableName, string details, CancellationToken cancellationToken);
    }
}
