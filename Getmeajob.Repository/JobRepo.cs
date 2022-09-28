using Getmeajob.Data;
using Getmeajob.Interface;
using Getmeajob.Model;
using Getmeajob.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.Repository
{
    public class JobRepo : IJob
    {
        private readonly GetmeajobDbContext _dbContext;
        public JobRepo(GetmeajobDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> Create(JobM t)
        {
            t.CreatedDate = DateTime.Now;
            _dbContext.Jobs.Add(t);
            await _dbContext.SaveChangesAsync();

            return t.JobId;
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JobM>> GetAll()
        {
            return await _dbContext
                .Jobs
                .Where(u => u.IsDeleted == false)
                .Include(i => i.user)
                .Include(i => i.company)
                .ToListAsync();
        }

        public async Task<JobM> GetById(int id)
        {
            return await _dbContext
               .Jobs
               .Where(u => u.IsDeleted == false && u.JobId == id)
               .Include(i => i.user)
               .Include(i => i.company)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<JobM>> GetByJobTitleOrLocation(JobSearchVM search)
        {
            return await _dbContext
               .Jobs
               .Where(u => u.IsDeleted == false && u.JobTitle.Contains(search.Name) || u.company.StreetAddress.Contains(search.Location) || u.company.City.Contains(search.Location) || u.company.State.Contains(search.Location) || u.company.Zip.Contains(search.Location) || u.company.Country.Contains(search.Location))
               .Include(i => i.user)
               .Include(i => i.company)
               .ToListAsync();
        }

        public Task<int> Update(JobM t)
        {
            throw new NotImplementedException();
        }
    }
}
