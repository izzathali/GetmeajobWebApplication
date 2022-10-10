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

        public async Task<int> Delete(int id)
        {
            JobM j = _dbContext.Jobs.FirstOrDefault(j => j.JobId == id);
            j.IsDeleted = true;

            _dbContext.Jobs.Update(j);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAllByUserId(int id)
        {
            var jobs = await _dbContext.Jobs
                .Where(j => j.IsDeleted == false && j.UserId == id)
                .ToListAsync();

            foreach (var j in jobs)
            {
                j.IsDeleted = true;
            }

            _dbContext.Jobs.UpdateRange(jobs);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<JobM>> GetAll()
        {
            return await _dbContext
                .Jobs
                .Where(u => u.IsDeleted == false && u.IsEmailVerified == true && u.IsApproved == true)
                .Include(i => i.user)
                .Include(i => i.company)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobM>> GetAllByUserId(int id)
        {
            return await _dbContext
               .Jobs
               .Where(u => u.IsDeleted == false && u.UserId == id && u.IsEmailVerified == true && u.IsApproved == true)
               .Include(i => i.user)
               .Include(i => i.company)
               .ToListAsync();
        }

        public async Task<IEnumerable<JobM>> GetAllUnapproved()
        {
            return await _dbContext
                .Jobs
                .Where(u => u.IsDeleted == false && u.IsEmailVerified == true && u.IsApproved == false)
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

        public async Task<JobM> GetByJobCode(Guid code)
        {
            return await _dbContext
               .Jobs
               .Where(u => u.IsDeleted == false && u.JobCode == code)
               .Include(i => i.user)
               .Include(i => i.company)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<JobM>> GetByJobTitleOrLocation(JobSearchVM search)
        {
            return await _dbContext
               .Jobs
               .Where(u => u.IsDeleted == false && u.IsEmailVerified == true && u.IsApproved == true && u.JobTitle.Contains(search.Name) || u.company.StreetAddress.Contains(search.Location) || u.company.City.Contains(search.Location) || u.company.State.Contains(search.Location) || u.company.Zip.Contains(search.Location) || u.company.Country.Contains(search.Location))
               .Include(i => i.user)
               .Include(i => i.company)
               .ToListAsync();
        }

        public async Task<JobM> GetByUserId(int id)
        {
            return await _dbContext.Jobs
                .Where(i => i.IsDeleted == false && i.UserId == id && i.IsEmailVerified == true && i.IsApproved == true)
                .Include(i => i.company)
                .FirstOrDefaultAsync();
        }

        public async Task<JobM> GetUnapprovedByUserId(int id)
        {
            return await _dbContext
               .Jobs
               .Where(u => u.IsDeleted == false && u.UserId == id && u.IsApproved == false)
               .Include(i => i.user)
               .Include(i => i.company)
               .FirstOrDefaultAsync();
        }

        public async Task<int> Update(JobM t)
        {
            _dbContext.Jobs.Update(t);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
