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
    public class UserRepo : IUser
    {
        private readonly GetmeajobDbContext _dbContext;
        public UserRepo(GetmeajobDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> Create(UserM t)
        {
            t.CreatedDate = DateTime.Now;
            _dbContext.Users.Add(t);
            await _dbContext.SaveChangesAsync();

            return t.UserId;
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserM>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserM> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(UserM t)
        {
            throw new NotImplementedException();
        }
    }
}
