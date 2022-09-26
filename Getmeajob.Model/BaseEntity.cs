using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.Model
{
    public class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
