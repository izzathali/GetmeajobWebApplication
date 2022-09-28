using Getmeajob.Model;
using Getmeajob.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.Interface
{
    public interface IJob : ICrud<JobM>
    {
        public Task<IEnumerable<JobM>> GetByJobTitleOrLocation(JobSearchVM search);
    }
}
