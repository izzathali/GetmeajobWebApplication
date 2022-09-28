using Getmeajob.Data;
using Getmeajob.Interface;
using Getmeajob.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.Repository
{
    public class JobSeekerRepo : IJobSeeker
    {
        private readonly GetmeajobDbContext _dbContext;
        public JobSeekerRepo(GetmeajobDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> Create(JobSeekerM t)
        {
            t.CreatedDate = DateTime.Now;
            _dbContext.JobSeekers.Add(t);
            await _dbContext.SaveChangesAsync();

            return t.JobSeekerId;
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JobSeekerM>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<JobSeekerM> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(JobSeekerM t)
        {
            throw new NotImplementedException();
        }
    }
}
