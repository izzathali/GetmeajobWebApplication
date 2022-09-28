﻿using Getmeajob.Data;
using Getmeajob.Interface;
using Getmeajob.Model;
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

        public Task<IEnumerable<JobM>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<JobM> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(JobM t)
        {
            throw new NotImplementedException();
        }
    }
}
