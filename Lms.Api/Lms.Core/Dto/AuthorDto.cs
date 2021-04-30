using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.API.Core.Entities;

namespace Lms.API.Core.Dto
{
    public class AuthorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public TimeSpan Age { get; set; }

        public IQueryable<Literature> Bibliography { get; set; }
    }
}
