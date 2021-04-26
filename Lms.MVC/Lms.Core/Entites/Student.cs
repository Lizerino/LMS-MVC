using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Entites
{
    class Student : User
    {

        public Course Course { get; set; }

    }
}
