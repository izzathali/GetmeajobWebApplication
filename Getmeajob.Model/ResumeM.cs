using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.Model
{
    public class ResumeM : BaseEntity
    {
        [Key]
        public int ResumeId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Resume { get; set; } = string.Empty;
        public decimal? LowerBound { get; set; }
        public decimal? UpperBound { get; set; }
        public string? Currency { get; set; }
        public string? OtherCurrency { get; set; }
        public int JobSeekerId { get; set; }
        public virtual JobSeekerM? jobseeker { get; set; }
        public int UserId { get; set; }
        public virtual UserM? user { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsApproved { get; set; }
        public Guid ResumeCode { get; set; }

        [NotMapped]
        public bool IsTermsAccepted { get; set; }
        [NotMapped]
        public string? Source { get; set; }
    }
}
