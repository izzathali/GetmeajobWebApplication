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

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResumeM>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ResumeM> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(ResumeM t)
        {
            throw new NotImplementedException();
        }
    }
}
