using Getmeajob.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.Interface
{
    public interface IUser : ICrud<UserM>
    {
        public Task<UserM> GetByEmailAndPass(UserM u);
        public Task<UserM> GetByEmail(UserM u);
        public Task<UserM> GetByCode(Guid code);
    }
}
