using Getmeajob.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.Interface
{
    public interface IResume : ICrud<ResumeM>
    {
        public Task<ResumeM> GetByUserId(int id);
        public Task<IEnumerable<ResumeM>> GetAllByUserId(int id);
    }
}
