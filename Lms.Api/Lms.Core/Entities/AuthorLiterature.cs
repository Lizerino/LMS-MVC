using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Core.Entities
{
    public class AuthorLiterature
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int LiteratureId { get; set; }
        public Literature Literature { get; set; }
    }
}
