using Getmeajob.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.Interface
{
    public interface IEmail
    {
        public void SendEmail(EmailVM e);
    }
}
