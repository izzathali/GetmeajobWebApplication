using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.ViewModel
{
    public class QuestionVM
    {
        public int? QuestionNo { get; set; }
        public ContactVM? contactVM { get; set; }
        public ForgotPassVM? forgotPassVM { get; set; }
    }
}
