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
    public class CompanyRepo : ICompany
    {
        private readonly GetmeajobDbContext _dbContext;
        public CompanyRepo(GetmeajobDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> Create(CompanyM t)
        {
            t.CreatedDate = DateTime.Now;
            _dbContext.Companies.Add(t);
            return await _dbContext.SaveChangesAsync();
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CompanyM>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CompanyM> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(CompanyM t)
        {
            throw new NotImplementedException();
        }
    }
}
