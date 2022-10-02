using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.Model
{
    public class CompanyM : BaseEntity
    {
        [Key]
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Telephone { get; set; }
        public string? Fax { get; set; }
    }
}
