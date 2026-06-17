using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Healthcare.Domain.Base;

namespace Healthcare.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }

        public required DateOnly DateOfBirth { get; set; }
        public required string ReasonForVisit { get; set; }

        public Guid CreatedByUserId { get; set; }

        public User? CreatedByUser { get; set; }

    }
}
