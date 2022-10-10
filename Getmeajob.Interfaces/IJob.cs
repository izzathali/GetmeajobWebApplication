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
        public Task<JobM> GetByUserId(int id);
        public Task<JobM> GetUnapprovedByUserId(int id);
        public Task<JobM> GetByJobCode(Guid code);
        public Task<IEnumerable<JobM>> GetAllByUserId(int id);
        public Task<int> DeleteAllByUserId(int id);
        public Task<IEnumerable<JobM>> GetAllUnapproved();
    }
}
