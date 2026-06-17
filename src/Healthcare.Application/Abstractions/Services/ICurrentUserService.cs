using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Abstractions.Services
{
    public interface ICurrentUserService
    {
        Guid? UserId {  get; }
    }
}
