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
    public class ResumeRepo : IResume
    {
        private readonly GetmeajobDbContext _dbContext;
        public ResumeRepo(GetmeajobDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> Create(ResumeM t)
        {
            t.CreatedDate = DateTime.Now;
            _dbContext.Resumes.Add(t);
            await _dbContext.SaveChangesAsync();

            return t.ResumeId;
        }

        public async Task<int> Delete(int id)
        {
            ResumeM r = _dbContext.Resumes.FirstOrDefault(j => j.ResumeId == id);
            r.IsDeleted = true;

            _dbContext.Resumes.Update(r);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAllByUserId(int id)
        {

            var resumes = await _dbContext.Resumes
               .Where(j => j.IsDeleted == false && j.UserId == id)
               .ToListAsync();

            foreach (var r in resumes)
            {
                r.IsDeleted = true;
            }

            _dbContext.Resumes.UpdateRange(resumes);

            return await _dbContext.SaveChangesAsync();
        }

        public Task<IEnumerable<ResumeM>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ResumeM>> GetAllByUserId(int id)
        {
            return await _dbContext
              .Resumes
              .Where(u => u.IsDeleted == false && u.UserId == id)
              .Include(i => i.user)
              .Include(i => i.jobseeker)
              .ToListAsync();

        }

        public async Task<ResumeM> GetById(int id)
        {
            return await _dbContext
              .Resumes
              .Where(u => u.IsDeleted == false && u.ResumeId == id)
              .Include(i => i.user)
              .Include(i => i.jobseeker)
              .FirstOrDefaultAsync();
        }

        public async Task<ResumeM> GetByJobCode(Guid code)
        {
            return await _dbContext
             .Resumes
             .Where(u => u.IsDeleted == false && u.ResumeCode == code)
             .Include(i => i.user)
             .Include(i => i.jobseeker)
             .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ResumeM>> GetByJobTitleOrLocation(JobSearchVM search)
        {
            return await _dbContext
              .Resumes
              .Where(u => u.IsDeleted == false && u.JobTitle.Contains(search.Name) || u.jobseeker.StreetAddress.Contains(search.Location) || u.jobseeker.City.Contains(search.Location) || u.jobseeker.State.Contains(search.Location) || u.jobseeker.Zip.Contains(search.Location) || u.jobseeker.Country.Contains(search.Location))
              .Include(i => i.user)
              .Include(i => i.jobseeker)
              .ToListAsync();
        }

        public async Task<ResumeM> GetByUserId(int id)
        {
            return await _dbContext.Resumes
              .Where(i => i.IsDeleted == false && i.UserId == id)
              .Include(i => i.jobseeker)
              .FirstOrDefaultAsync();
        }

        public async Task<int> Update(ResumeM t)
        {
            _dbContext.Resumes.Update(t);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
